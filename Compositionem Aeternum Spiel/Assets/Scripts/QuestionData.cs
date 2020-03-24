using System.Linq;
using UnityEngine;

[System.Serializable]
public class QuestionData
{
    public int questionID;
	public string questionText;
	public AnswerData[] answers;
}

public static class RandomExtensions
{
    public static void Shuffle<T> (this T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = Random.Range(0, n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}