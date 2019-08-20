using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BENumber
///   Description:    this is utility calss to manupulate numbers
///                   when value changes, display values change to target value gradually
///                   and create string from values by several types.
///   Usage :		  
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-08-30)
///-----------------------------------------------------------------------------------------
namespace BE {

	public class BENumber {
	
		public enum IncType {
			NONE 			= -1,
			VALUE 			= 0, //ex) 1,000,000
			VALUEwithMAX 	= 1, //ex) 56/300
			TIME 			= 2, //ex) 5d 1h
		};
		
		private bool  	bInChange 	= false;
		private float 	fAge 		= 0.0f;
		private float 	fInc 		= 1.0f;
		private double 	fTarget 	= 0.0;
		
		private double 	fMin 		= 0.0;
		private double 	fMax 		= 1.0;
		private double 	fCurrent 	= 0.0;
		
		private IncType eType		= IncType.VALUEwithMAX;
		
		private GameObject 		m_EventTarget = null;
		private string 			m_EventFunction;
		private GameObject 		m_EventParameter;

		private Text			UIText = null;
		private Image			UIImage = null;

		public BENumber(IncType type, double min, double max, double current) {
			Init(type, min, max, current);
		}
		
		public void Init(IncType type, double min, double max, double current) {
			eType = type;
			fMin = min;
			fMax = max;
			fCurrent = current;
			fTarget = current;
			bInChange = false;
		}

		public void 	AddUIText(Text ui)		{ UIText = ui; if(UIText != null) UIText.text = ToString(); }
		public void 	AddUIImage(Image ui)	{ UIImage = ui; if(UIImage != null) UIImage.fillAmount = Ratio(); }

		public void 	TypeSet(IncType type)	{ eType = type; }
		public IncType 	Type()					{ return eType; }
		
		public bool 	InChange()				{ return bInChange; }
		public float 	Ratio()					{ return (float)((fCurrent-fMin)/(fMax-fMin)); }
		public float 	TargetRatio()			{ return (float)((fTarget-fMin)/(fMax-fMin)); }
		public double 	Current()				{ return fCurrent; }
		public double 	Min()					{ return fMin; }
		public double 	Max()					{ return fMax; }
		public void 	MaxSet(double value)	{ fMax = value; UpdateUI(); }
		public double 	Target()				{ return fTarget; }
		public override string 	ToString()		{ 
			if(eType == IncType.VALUE) 				return ((int)fCurrent).ToString ("#,##0"); 
			else if(eType == IncType.VALUEwithMAX) 	return ((int)fCurrent).ToString ("#,##0")+" / "+((int)fMax).ToString ("#,##0"); 
			else if(eType == IncType.TIME) 			return SecToString((int)fCurrent);
			else 									return "";
		}

		public static string  SecToString(int sec) {
			int iCurrent = sec;
			int Day  = iCurrent/86400;	if(Day > 0)  iCurrent -= Day *86400;
			int Hour = iCurrent/3600;	if(Hour > 0) iCurrent -= Hour*3600;
			int Min  = iCurrent/60;		if(Min > 0)  iCurrent -= Min*60;
			int Sec  = iCurrent;
			
			if(Day > 0) 		return Day.ToString()+ "D "+((Hour != 0) ? Hour.ToString ()+"H" : "");
			else if(Hour > 0) 	return Hour.ToString()+"H "+((Min  != 0) ? Min.ToString ()+"M" : "");
			else if(Min > 0) 	return Min.ToString()+ "M "+((Sec  != 0) ? Sec.ToString ()+"S" : "");
			else 				return Sec.ToString ()+"S";
		}

		public void ChangeTo(double target) {
			if(target < fMin) target = fMin;
			if(target > fMax) target = fMax;
			if(!bInChange) {
				bInChange = true;
				fAge = 0.0f;
				fInc = 1.0f;
			}
			fTarget = target;
		}

		public void ChangeDelta(double target) {
			ChangeTo(fTarget+target);
		}

		public void Update() {
			if(!bInChange) return;

			// for number increase, decrease animation
			fAge += Time.deltaTime * 6.0f;
			fInc += Mathf.Exp(fAge);
			
			if(fTarget > fCurrent) 	{ fCurrent += (double)fInc; if(fCurrent >= fTarget) End(); }
			else  					{ fCurrent -= (double)fInc; if(fCurrent <= fTarget) End(); }

			UpdateUI();
		}

		public void UpdateUI() {
			if(UIText != null) UIText.text = ToString();
			if(UIImage != null) UIImage.fillAmount = Ratio();
		}

		private void End() {
			bInChange = false; 
			fCurrent = fTarget;
			
			if(m_EventTarget != null)
				m_EventTarget.SendMessage(m_EventFunction, m_EventParameter);
		}

		public void SetReceiver(GameObject target, string functionName, GameObject parameter) {
			m_EventTarget = target;
			m_EventFunction = functionName;
			m_EventParameter = parameter;
		}
	}

}
