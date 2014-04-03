using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class Evvent
{
	public float HappeningTime=0.0f;

	public System.DateTime Date;

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

	public System.DateTime Start;
	public System.DateTime End;

	public Tome(string _Name, string _CodeName, int _Order)
	{
		this.Name = _Name;
		this.CodeName = _CodeName;
		this.Order = _Order;
	}

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

	public Dictionary<DateTime, string> DeathSentences;

    private static readonly CLS_Database instance = new CLS_Database();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static CLS_Database()
    {
    }

    private CLS_Database()
    {
		TomeDict = new Dictionary<string,Tome>();
		DeathSentences = new Dictionary<DateTime, string>();

		Tome FULL = new Tome("A SONG OF ICE AND FIRE","ASOIAF",0);

		Tome agot = new Tome("A GAME OF THRONES","AGOT",1);
		Tome acok = new Tome("A CLASH OF KINGS","ACOK",2);
		Tome asos = new Tome("A STORM OF SWORDS","ASOS",3);
		Tome affc = new Tome("A FEAST FOR CROWS","AFFC",4);
		Tome adwd = new Tome("A DANCE WITH DRAGONS","ADWD",5);

		FULL.Start = new DateTime( 298,2,24 );
		FULL.End = new DateTime( 300,7,31 );

		agot.Start = new DateTime( 298,2,24 );
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

		DeathSentences[new DateTime(298,11,15) ] = "Robert";
		DeathSentences[new DateTime(298,12,23) ] = "Ned";
		DeathSentences[new DateTime(299,7,15) ] = "Renly";
		DeathSentences[new DateTime(299,12,23) ] = "Catelyn";
		DeathSentences[new DateTime(299,12,23) ] = "Robb";
		DeathSentences[new DateTime(300,1,1) ] = "Joffrey";
		DeathSentences[new DateTime(300,1,24) ] = "Tywin";
		DeathSentences[new DateTime(300,1,23) ] = "Lysa"; //This one NEEDS CONFIRMATION


		//ReadDatabase();
    }

    public static CLS_Database Instance
    {
        get
        {
            return instance;
        }
    }

	//public string[] getDeath(DateTime _Date)
	public bool amIDead(DateTime _Date,string _CharacterName)
	{
		foreach (KeyValuePair<DateTime, string> kvp in this.DeathSentences) 
		{
			if(kvp.Value==_CharacterName)
				if( kvp.Key < _Date )
					return true;
		}
		return false;
	}

}
