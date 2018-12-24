using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class XmlProcessor{
	TextAsset file;
	string text;
	string node;
	int i;

	public XmlProcessor(string filename){
		i = 0;
		file = Resources.Load(filename) as TextAsset;
		text = file.text;
	}

	public bool IsDone(){
		return i >= text.Length;
	}

	private bool isWhiteSpace(char c){
		return c == ' ' || c == '	' || c == '\n' || c == '\r';
	}

	public string getNextNode(){
		string buffer = "";
		while(text[i] != '<'){
			i++;
		}
		i++;
		while(!isWhiteSpace(text[i]) && text[i]!='>'){
			buffer = buffer + text[i];
			i++;
		}
		i++;
		return buffer;
	}

	public XmlAttribute getNextAttribute(){
		XmlAttribute attribute = new XmlAttribute();
		while(text[i]!='"' && text[i]!='/' && text[i]!='>'){
			if(!isWhiteSpace(text[i]) && text[i] != '='){
				attribute.name = attribute.name + text[i];
			}
			i++;
		}
		if(text[i] == '"'){
			i++;
			while(text[i]!='"'){
				attribute.value = attribute.value + text[i];
				i++;
			}
			i++;
		}else{
			i+=2;
		}
		return attribute;
	}
}

public class XmlWriter{
	public void writeToFile(string outputPath, List<XmlNode> nodes){
		outputPath = getLocalPath() + outputPath;
		Debug.Log(outputPath);
		FileStream output;
		string text = "";
		output = File.OpenWrite(outputPath);
		for(int i=0;i<nodes.Count;i++){
			text = text + nodes[i].toString();
		}
		output.Write(new UTF8Encoding(true).GetBytes(text), 0, text.Length);
		output.Close();
	}

	public void writeToFile(string outputPath, XmlNode node){
		outputPath = getLocalPath() + outputPath;
		Debug.Log(outputPath);
		FileStream output;
		string text = node.toString();
		output = File.OpenWrite(outputPath);
		output.Write(new UTF8Encoding(true).GetBytes(text), 0, text.Length);
		output.Close();
	}

	string getLocalPath(){
		string path = Application.dataPath;
		#if UNITY_STANDALONE_WIN
			path = path + "/../";
		#endif
		#if UNITY_STANDALONE_OSX
			path = path + "/../../";
		#endif
		#if UNITY_STANDALONE_LINUX
			path = path + "/../../";
		#endif

		return path;
	}

	public void concatenateFiles(string outputPath, List<string> inputPaths){
		string text = "";
		FileStream input;
		Debug.Log(getLocalPath());
	}

	public void concatenateFiles(string outputPath, string inputMask){
		string text = "";
		FileStream input;
		Debug.Log(getLocalPath());
	}
}

public class XmlAttribute {
	public string name;
	public string value;
	public XmlAttribute(){
		name = "";
		value = "";
	}
	public XmlAttribute(string n, string v){
		name = n;
		value = v;
	}
}

public class XmlNode {
	public string name = "";
	public List<XmlAttribute> attributes = new List<XmlAttribute>();
	public List<XmlNode> subnodes = new List<XmlNode>();

	public string toString(){
		string output = "";
		output = output + "<" + name;
		for(int i=0;i<attributes.Count;i++){
			output = output + "\n" + attributes[i].name + "=" + "\"" + attributes[i].value + "\"";
		}
		output = output + ">" + "\n";
		for(int i=0;i<subnodes.Count;i++){
			output = output + subnodes[i].toString();
		}
		output = output + "</" + name + ">\n";
		return output;
	}
}