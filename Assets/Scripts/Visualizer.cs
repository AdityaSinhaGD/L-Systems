using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public LSystemBuilder lSystemBuilder;

    List<Vector3> positions = new List<Vector3>();

    public GameObject prefab;

    public Material lineMaterial;

    private int length = 10;

    private float rotationAngle = 90;

    /*Want to Reduce length the further we go*/
    public int Length {
        
        get
        {
            if(length > 0)
            {
                return length;
            }
            else
            {
                return 1;
            }
        }
        set => length = value;
    }

    private void Start()
    {
        string sequence = lSystemBuilder.Generate();

        VisualizeSequence(sequence);
    }

    private void VisualizeSequence(string sequence)
    {
        Stack<LAgentParams> cachedPoints = new Stack<LAgentParams>();

        Vector3 currentPosition = Vector3.zero;

        Vector3 direction = Vector3.forward;

        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

        foreach(char c in sequence)
        {
            Encoding encoding = (Encoding)c;

            switch (encoding)
            {
               
                case Encoding.save:
                    cachedPoints.Push(new LAgentParams
                    {
                        position = currentPosition,
                        direction = direction,
                        length = Length

                    });
                    break;
                case Encoding.load:
                    if (cachedPoints.Count > 0)
                    {
                        var agentParam = cachedPoints.Pop();

                        currentPosition = agentParam.position;

                        direction = agentParam.direction;

                        Length = agentParam.length;
                    }
                    else
                    {
                        throw new System.Exception("Out of cached points");
                    }
                    break;
                case Encoding.draw:
                    tempPosition = currentPosition;

                    currentPosition += direction * length;

                    Drawline(tempPosition, currentPosition, Color.red);

                    Length -= 1;

                    positions.Add(currentPosition);
                    break;
                case Encoding.turnRight:
                    direction = Quaternion.AngleAxis(rotationAngle, Vector3.up) * direction;
                    break;
                case Encoding.turnLeft:
                    direction = Quaternion.AngleAxis(-rotationAngle, Vector3.up) * direction;
                    break;
                default:
                    break;
            }
        }

        foreach(var position in positions)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }

    }

    private void Drawline(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("line");

        line.transform.position = start;

        var lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.material = lineMaterial;

        lineRenderer.startColor = color;

        lineRenderer.endColor = color;

        lineRenderer.startWidth = 0.5f;

        lineRenderer.endWidth = 0.5f;

        lineRenderer.SetPosition(0, start);

        lineRenderer.SetPosition(1, end);
    }

    /*Original material has a Move symbol but in this implementation we are appending a full line so the symbol is not needed*/
    public enum Encoding
    {
        terminate = '1',
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-'
    }
}
