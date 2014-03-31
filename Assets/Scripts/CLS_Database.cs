using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class Evvent
{
	public float HappeningTime=0.0f;

	//public int Day;
	//public int Month;
	//public int Year;
	public System.DateTime Date;

	//public int BookIndex;	//Tome NUmber
	public Tome Book =null;

	public string Name="Default";
	public string Info="";

	public GameObject Location;
	public List<GameObject> Characters;

	public int ChapterIndex;
	public string ChapterCharacter="Default";
	public string ChapterCode="Default";

	public Evvent()
	{
		this.Characters = new List<GameObject>();
	}
}

public class Tome
{
	public string CodeName;
	public string Name;
	public int Order;

	//public int Start_Day;
	//public int Start_Month;
	//public int Start_Year;

	//public int End_Day;
	//public int End_Month;
	//public int End_Year;

	public System.DateTime Start;
	public System.DateTime End;

	public Tome(string _Name, string _CodeName, int _Order)
	{
		this.Name = _Name;
		this.CodeName = _CodeName;
		this.Order = _Order;
	}

	//float getRatio(int _Year, int _Month, int _Day)
	public float getRatio(System.DateTime _TheDate)
	{
		System.TimeSpan delta = _TheDate - this.Start;
		System.TimeSpan range = this.End - this.Start;

		return (float) delta.TotalSeconds /  (float) range.TotalSeconds;
	}
}

public sealed class CLS_Database
{
	//public List<Tome> TomeList;
	public Dictionary<string,Tome> TomeDict;

    private static readonly CLS_Database instance = new CLS_Database();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static CLS_Database()
    {
    }

    private CLS_Database()
    {
		TomeDict = new Dictionary<string,Tome>();

		Tome FULL = new Tome("A SONG OF ICE AND FIRE","ASOIAF",0);

		Tome agot = new Tome("A GAME OF THRONES","AGOT",1);
		Tome acok = new Tome("A CLASH OF KINGS","ACOK",2);
		Tome asos = new Tome("A STORM OF SWORDS","ASOS",3);
		Tome affc = new Tome("A FEAST FOR CROWS","AFFC",4);
		Tome adwd = new Tome("A DANCE WITH DRAGONS","ADWD",5);

		FULL.Start = new DateTime( 297,4,22 );
		FULL.End = new DateTime( 300,7,31 );

		agot.Start = new DateTime( 297,4,22 );
		agot.End = new DateTime( 299,1,20 );
		acok.Start = new DateTime( 299,1,22 );
		acok.End = new DateTime( 299,10,27 );
		asos.Start = new DateTime( 299,9,9 );
		asos.End = new DateTime( 300,2,10 );
		affc.Start = new DateTime( 299,12,5 );
		affc.End = new DateTime( 300,5,21 );
		adwd.Start = new DateTime( 300,1,5 );
		adwd.End = new DateTime( 300,7,31 );

		TomeDict["ASOIAF"] = FULL;
		TomeDict["AGOT"] = agot;
		TomeDict["ACOK"] = acok;
		TomeDict["ASOS"] = asos;
		TomeDict["AFFC"] = affc;
		TomeDict["ADWD"] = adwd;





		//ReadDatabase();
    }

    public static CLS_Database Instance
    {
        get
        {
            return instance;
        }
    }
	
//	static string getTomeTile(int Index)
//	{
//		return CLS_Database.instance.TomeList[Index].Name;
//	}

//	static int getTomeIndex(string BookCode)
//	{
//		switch (BookCode) 
//		{
//				default: return null;break;
//				case "AGOT": return 0;
//				case "ACOK": return 1;
//				case "ASOS": return 2;
//				case "AFFC": return 3;
//				case "ADWD": return 4;
//		}
//	}

	/*
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
	*/
}
