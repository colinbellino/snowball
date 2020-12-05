using System;

[Serializable]
public class Barks
{
	public Bark[] Recruited;
	public Bark[] Died;
	public Bark[] AllyDied;
	public Bark[] LevelFinished;
}

[Serializable]
public class Bark
{
	public string Text;
}
