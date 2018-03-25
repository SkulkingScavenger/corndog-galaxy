using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public string getNextAttribute(){
		string attributeValue = "";
		while(text[i]!='"' && text[i]!='/'){
			i++;
		}
		if(text[i] == '"'){
			i++;
			while(text[i]!='"'){
				attributeValue = attributeValue + text[i];
				i++;
			}
			i++;
		}else{
			i+=2;
		}
		return attributeValue;
	}
}