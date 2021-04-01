using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using UnityEditor;
public class CharacterProcessor : MonoBehaviour
{
    public TextAsset originText;
    public TextAsset[] bluePrintTexts;
    private void Start()
    {
        string origin = RemoveDuplicateCharacter(originText.text);
        string[] blueprints = new string[bluePrintTexts.Length];
        for (int i = 0; i < bluePrintTexts.Length; i++)
            blueprints[i] = RemoveDuplicateCharacter(bluePrintTexts[i].text);//remove all duplicate characters.
        string outputText = GetCharacterNotInBluePrint(origin, blueprints);//only keep character which is not included in blueprints 
        outputText = SortString(outputText);//sort the string
        string dataPath = Path.Combine(Application.streamingAssetsPath, originText.name + "_output.txt");//save output.txt to streaming asset folder
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);
        File.WriteAllText(dataPath, outputText);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
    private string SortString(string origin)
    {
        char[] characters = origin.ToCharArray();
        System.Array.Sort(characters);
        return new string(characters);
    }
    private string RemoveDuplicateCharacter(string origin)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = origin.Length - 1; i > -1; i--)
        {
            bool hasChar = false;
            for (int j = sb.Length - 1; j > -1; j--)
                if (sb[j] == origin[i])
                {
                    hasChar = true;
                    break;
                }
            if (!hasChar)
                sb.Append(origin[i]);
        }
        return sb.ToString();
    }
    private string GetCharacterNotInBluePrint(string origin, string[] blueprints)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = origin.Length - 1; i > -1; i--)
        {
            bool hasChar = false;
            foreach (string blueprint in blueprints)
                if (HasCharacter(blueprint, origin[i]))
                {
                    hasChar = true;
                    break;
                }
            if (!hasChar)
                sb.Append(origin[i]);
        }
        return sb.ToString();
    }
    private bool HasCharacter(string source, char character)
    {
        for (int i = source.Length - 1; i > -1; i--)
            if (source[i] == character) return true;
        return false;
    }
}