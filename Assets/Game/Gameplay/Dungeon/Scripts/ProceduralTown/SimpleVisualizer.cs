using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SVS
{
    public class SimpleVisualizer : MonoBehaviour
    {
        public LSystemGenerator lSystem;
        List<Vector3> positions = new List<Vector3>();
        [SerializeField]private GameObject prefab;
        [SerializeField]private Material lineMaterial;
        [SerializeField] private Transform pathContainer;



        private int lenght = 8;
        [SerializeField][Range(0f,90f)]private float angle = 90f;

        public int Lenght
        {
            get
            {
               return lenght > 0? lenght : 1;
            }
             set => lenght = value;
        }
        public enum EncodingLetters
        {
            unknown = '1',
            save = '[',
            load = ']',
            draw = 'F',
            turnRight = '+',
            turnLeft = '-'
        }
        
        private void Start() 
        {
           var sequence = lSystem.GenerateSentence();
           VisualizeSequence(sequence); 
        }

        private void VisualizeSequence(string sequence)
        {
            Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
            Vector3 currentPosition = Vector3.zero;

            Vector3 direction = Vector3.forward;
            Vector3 tempPosition = Vector3.zero;

            positions.Add(currentPosition);

            foreach(var letter in sequence)
            {
                EncodingLetters encoding = (EncodingLetters)letter;

                switch(encoding)
                {
                    case EncodingLetters.unknown:
                        break;
                    case EncodingLetters.save:
                        savePoints.Push( new AgentParameters
                        {
                            position = currentPosition,
                            direction = direction,
                            lenght = Lenght
                        });
                        break;
                    case EncodingLetters.load:
                        if(savePoints.Count > 0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            Lenght = agentParameter.lenght;
                        }
                        else
                        {
                            throw new System.Exception("Dont have saved point in Stack");
                        }
                        break;
                    case EncodingLetters.draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * lenght;
                       
                        DrawLine(tempPosition, currentPosition, Color.red);
                        Lenght -= 2;
                        positions.Add(currentPosition);
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        break;

                }
           
            }

            foreach(var position in positions)
            {
                Instantiate(prefab, position, Quaternion.identity, pathContainer);
            }
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            GameObject line = new GameObject("line");
            line.transform.SetParent(pathContainer);
            
            line.transform.position = start;
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0,start);
            lineRenderer.SetPosition(1,end);

        }
    }
}