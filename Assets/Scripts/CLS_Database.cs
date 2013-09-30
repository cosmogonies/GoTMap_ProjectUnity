using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Evvent
{
	public float HappeningTime=0.0f;
	public GameObject Location;
	public string Name="Default";
	public List<GameObject> Characters;
	public Evvent()
	{
		this.Characters = new List<GameObject>();
	}
}



public sealed class CLS_Database
{
	private string[] TomeList = new string[]{"A GAME OF THRONES","A CLASH OF KINGS","A STORM OF SWORDS","A FEAT FOR CROWS","A DANCE WITH DRAGONS"};

    private static readonly CLS_Database instance = new CLS_Database();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static CLS_Database()
    {
    }

    private CLS_Database()
    {
		ReadDatabase();
    }

    public static CLS_Database Instance
    {
        get
        {
            return instance;
        }
    }
	
	static string getTomeTile(int Index)
	{
		if(Index>0)
			if(Index< CLS_Database.instance.TomeList.Length)
				return CLS_Database.instance.TomeList[Index];
		return null;
	}

	static void ReadDatabase()
	{
		FileInfo theSourceFile = null;
		StringReader reader = null; 
		 
		TextAsset puzdata = (TextAsset)Resources.Load("Database_Tome1", typeof(TextAsset));
		Debug.Log (puzdata.text);
		reader = new StringReader(puzdata.text);
		if ( reader == null )
		{
		   Debug.LogError("Database_Tome1.txt not found or not readable");
		}
		else
		{
			string txt;
			while(        (txt = reader.ReadLine() ) != null )
			{
				Debug.Log("-->" + txt);
			}
		}
	}
	
}
