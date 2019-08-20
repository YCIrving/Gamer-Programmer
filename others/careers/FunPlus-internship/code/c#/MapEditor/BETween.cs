using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BETween
///   Description:    This class is implementation of simple tween function.
/// 				  simple  & easy to use.
///   Usage :		  BETween bt = BETween.scale(stars[i].gameObject, 0.2f, new Vector3(0.7f,0.7f,0.7f), new Vector3(1,1,1));
///                   bt.method = BETweenMethod.easeOut;
///                   bt.delay = fTimeStart;
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.3 (2015-10-03)
///                   - group control alpha added
///                   - pingpong error (pong not played) 
/// 				  v1.4 (2016-01-05)
///                   - pingpong loopcount apply
///-----------------------------------------------------------------------------------------
namespace BE {

	// what property to tween
	public enum BETweenType {
		position,
		rotation,
		scale,
		alpha,
		color,
		active,
		anchoredPosition3D,
		anchoredPosition,
		size,
	}

	// tween interpolation model
	public enum BETweenMethod {
		linear,
		easeIn,
		easeOut,
		easeInOut,
		easeOutIn,
		easeInBack,
		easeOutBack,
	}

	// tween direction
	public enum BETweenLoop {
		normal,
		pingpong,
		mirror,
	}
	
	public class BETween : MonoBehaviour {

		public 	BETweenType		type = BETweenType.position;
		public 	BETweenMethod	method = BETweenMethod.linear;
		public  int				loopCount = 1;			// how many times this tween run
		public 	BETweenLoop		loopStyle = BETweenLoop.normal;
		public 	float			delay = 0.0f;			// delay before start tween
		public 	float			duration = 0.0f;		// time length the tween proceed 
		public 	bool			ignoreTimeScale = true;	// tween not affected by Time.scale
		public 	Action	 		onStart = null;			// call action when tween start
		public 	Action<float>	onUpdate = null;		// call action when tween update - float value is completion ratio (0.0f ~ 1.0f)
		public 	Action	 		onFinish = null;		// call action when tween finished

		public 	GameObject 		goTarget=null;			// target gameobject to apply tween
		public 	Transform 		tr=null;				// transform of target gameobject
		public 	RectTransform 	rt=null;				// rectTransform of target gameobject
		public 	Image			image=null;
		public 	CanvasGroup		canvasGroup=null;
		public 	MeshRenderer	meshRender=null;


		public 	Vector3 		from;			// start value
		public 	Vector3 		to;				// end value
		public 	Vector3 		current;		// current value (calculated by tween functions)

		// variables for ignoreTimeScale
		float 	timeCurrent = 0f;
		float 	timeStart = 0f;
		float 	timeDelta = 0f;
		float 	timeActual = 0f;

		float	timeAge = 0.0f;		// age after tween started
		bool	Increase = true;	// direction of tween
		bool	Started = false;	// tween started or not
		bool	End = false;		// tween ended or not
		bool	applyFrom = false;	// set start value to target, when tween begin

		// not affected by Time.scale
		protected float UpdateRealTimeDelta () {
			timeCurrent = Time.realtimeSinceStartup;
			timeActual += Mathf.Max(0f, timeCurrent - timeStart);
			timeDelta = 0.001f * Mathf.Round(timeActual * 1000f);
			timeActual -= timeDelta;
			if (timeDelta > 1f) timeDelta = 1f;
			timeStart = timeCurrent;

			return timeDelta;
		}

		void Start() {
			if(ignoreTimeScale) {
				timeDelta = 0f;
				timeStart = Time.realtimeSinceStartup;
			}
		}

		void Update () {
		
			float delta = ignoreTimeScale ? UpdateRealTimeDelta() : Time.deltaTime;

			if(delay > 0.0f) {
				delay -= delta;
				if(delay < 0.0f) {
					timeAge = -delay;
					ApplyStart();
				}
				else 				
					return;
			}
			else {
				if(!Started) 
					ApplyStart();

				timeAge += delta;
			}

			float ratio = timeAge/duration;
			if(ratio > 1.0f) {

				ratio -= 1.0f;
				timeAge -= duration;

				if(loopStyle == BETweenLoop.normal) {
					End = true;
				}
				else if(loopStyle == BETweenLoop.pingpong) {
					Increase = !Increase;
					if(Increase)
						End = true;
				}
				else {}

			}

			float ratioFinal = ratio;
			if((loopStyle == BETweenLoop.pingpong) && !Increase) {
				ratioFinal = 1.0f-ratio;
			}

			if(onUpdate != null)
				onUpdate.Invoke(ratioFinal);
			
			if(End) {
				End = false;
				loopCount--;

				if(type == BETweenType.active) {
					goTarget.SetActive((to.x > 0.5f) ? true : false);
				}

				if(loopCount == 0) {
					ratioFinal = (loopStyle == BETweenLoop.pingpong) ? 0.0f : 1.0f;
				}
			}

			//apply value
			ratioFinal = Transition(method, ratioFinal);
			current.x = (to.x - from.x)*ratioFinal + from.x;
			current.y = (to.y - from.y)*ratioFinal + from.y;
			current.z = (to.z - from.z)*ratioFinal + from.z;
			ApplyValue(current);

			// end notify
			if(loopCount == 0) {

				if(onFinish != null)
					onFinish.Invoke();

				Clear ();
			}
		}

		public void ApplyStart() {
			Started = true;
			if(applyFrom) 
				ApplyValue(from);

			if(onStart != null)
				onStart.Invoke();
		}

		public void ApplyValue(Vector3 value) {
			if(type == BETweenType.position) {
				tr.localPosition = value;
			}
			else if(type == BETweenType.rotation) {
				tr.localRotation = Quaternion.Euler(value);
			}
			else if(type == BETweenType.scale) {
				tr.localScale = value;
			}
			else if(type == BETweenType.alpha) {
				if(canvasGroup != null) 		{ canvasGroup.alpha = value.x; }
				else if(image != null) 			{ Color color = image.color;	color.a = value.x;	image.color = color; }
				else if(meshRender != null) 	{
					for(int i=0 ; i < meshRender.materials.Length ; ++i) {
						/*Color color = meshRender.materials[i].GetColor("_TintColor");
						color.a = value.x;
						meshRender.materials[i].SetColor("_TintColor", color);*/
					}
				}
				else {}
			}
			else if(type == BETweenType.color) {
				if(image != null) 				{ Color color = image.color;	color.r = value.x;	color.g = value.y;	color.b = value.z;	image.color = color; }
				else if(meshRender != null) 	{
					for(int i=0 ; i < meshRender.materials.Length ; ++i) {
						/*Color color = meshRender.materials[i].GetColor("_TintColor");
						color.r = value.x;
						color.g = value.y;
						color.b = value.z;
						meshRender.materials[i].SetColor("_TintColor", color);*/
					}
				}
				else {}
			}
			else if(type == BETweenType.active) {
				goTarget.SetActive((value.x > 0.5f) ? true : false);
			}
			else if(type == BETweenType.anchoredPosition3D) {
				rt.anchoredPosition3D = value;
			}
			else if(type == BETweenType.anchoredPosition) {
				rt.anchoredPosition = new Vector3(value.x,value.y);
			}
			else if(type == BETweenType.size) {
				rt.sizeDelta = new Vector2(value.x,value.y);
			}
			else {}
		}

		public void Clear() {
			Destroy (this);
		}

		public float Transition(BETweenMethod _method, float ratio) {
			switch(_method) {
			case BETweenMethod.linear: {
					return ratio;
				}
			case BETweenMethod.easeIn: {
					return ratio * ratio * ratio;
				}
			case BETweenMethod.easeOut: {
					float invRatio = ratio - 1.0f;
					return invRatio * invRatio * invRatio + 1.0f;
				}
			case BETweenMethod.easeInOut: {
				if (ratio < 0.5f) return 0.5f * Transition(BETweenMethod.easeIn,ratio*2.0f);
				else              return 0.5f * Transition(BETweenMethod.easeOut,(ratio-0.5f)*2.0f) + 0.5f;
				}
			case BETweenMethod.easeOutIn: {
				if (ratio < 0.5f) return 0.5f * Transition(BETweenMethod.easeOut,ratio*2.0f);
				else              return 0.5f * Transition(BETweenMethod.easeIn,(ratio-0.5f)*2.0f) + 0.5f;
				}
			case BETweenMethod.easeInBack: {
					float s = 1.70158f;
					return Mathf.Pow(ratio, 2.0f) * ((s + 1.0f)*ratio - s);    
				}
			case BETweenMethod.easeOutBack: {
					float invRatio = ratio - 1.0f;
					float s = 1.70158f;
					return Mathf.Pow(invRatio, 2.0f) * ((s + 1.0f)*invRatio + s) + 1.0f;   
				}
				default: return 0.0f;
			}
		}

		public static BETween add(GameObject go, float _duration, BETweenType type) {

			// when creation, this script not attached to target,
			// instead, create gameobject named "BETween" and attach to this gameobject.
			// in this method, tween work well even target object is not active.
			GameObject goBase = GameObject.Find ("BETween");
			if(goBase == null) 
				goBase = new GameObject("BETween");

			BETween newTween = goBase.AddComponent<BETween>();
			newTween.goTarget = go;
			newTween.tr = go.transform;
			newTween.rt = go.GetComponent<RectTransform>();
			newTween.canvasGroup = go.GetComponent<CanvasGroup>();
			newTween.image = go.GetComponent<Image>();
			newTween.meshRender = go.GetComponent<MeshRenderer>();
			newTween.duration = _duration;
			newTween.type = type;

			return newTween;
		}

		public static BETween position(GameObject go, float _duration, Vector3 _from, Vector3 _pos) {
			BETween tween = add(go, _duration, BETweenType.position);
			tween.from = _from;
			tween.to = _pos;
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween position(GameObject go, float _duration, Vector3 _pos) {
			BETween tween = add(go, _duration, BETweenType.position);
			tween.from = tween.tr.localPosition;
			tween.to = _pos;
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}
		
		public static BETween rotation(GameObject go, float _duration, Vector3 _from, Vector3 _rot) {
			BETween tween = add(go, _duration, BETweenType.rotation);
			tween.from = _from;
			tween.to = _rot;
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween rotation(GameObject go, float _duration, Vector3 _rot) {
			BETween tween = add(go, _duration, BETweenType.rotation);
			tween.from = tween.tr.localRotation.eulerAngles;
			tween.to = _rot;
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}

		public static BETween scale(GameObject go, float _duration, Vector3 _from, Vector3 _scale) {
			BETween tween = add(go, _duration, BETweenType.scale);
			tween.from = _from;
			tween.to = _scale;
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween scale(GameObject go, float _duration, Vector3 _scale) {
			BETween tween = add(go, _duration, BETweenType.scale);
			tween.from = go.transform.localScale;
			tween.to = _scale;
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}

		public static BETween alpha(GameObject go, float _duration, float _from, float _alpha) {
			BETween tween = add(go, _duration, BETweenType.alpha);
			tween.from = new Vector3(_from,0,0);
			tween.to = new Vector3(_alpha,0,0);
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween alpha(GameObject go, float _duration, float _alpha) {
			BETween tween = add(go, _duration, BETweenType.alpha);

			Color color = Color.white;
			if(tween.canvasGroup != null) 		color.a = tween.canvasGroup.alpha;
			else if(tween.image != null) 		color = tween.image.color;
//			else if(tween.meshRender != null) 	color = tween.meshRender.materials[0].GetColor("_TintColor");
			else {}

			tween.from = new Vector3(color.a,0,0);
			tween.to = new Vector3(_alpha,0,0);
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}
		
		public static BETween color(GameObject go, float _duration, Color _from, Color _color) {
			BETween tween = add(go, _duration, BETweenType.color);
			tween.from = new Vector3(_from.r,_from.g,_from.b);
			tween.to = new Vector3(_color.r,_color.g,_color.b);
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween color(GameObject go, float _duration, Color _color) {
			BETween tween = add(go, _duration, BETweenType.color);
			
			Color color = Color.white;
			if(tween.image != null) 			color = tween.image.color;
//			else if(tween.meshRender != null) 	color = tween.meshRender.materials[0].GetColor("_TintColor");
			else {}

			tween.from = new Vector3(color.r,color.g,color.b);
			tween.to = new Vector3(_color.r,_color.g,_color.b);
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}
		
		public static BETween enable(GameObject go, float _duration, bool to, bool enable) {
			BETween tween = add(go, _duration, BETweenType.active);
			tween.from = new Vector3(to ? 1.0f : 0.0f,0.0f,0.0f);
			tween.to = new Vector3(enable ? 1.0f : 0.0f,0.0f,0.0f);
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween enable(GameObject go, float _duration, bool enable) {
			BETween tween = add(go, _duration, BETweenType.active);
			tween.from = Vector3.zero;
			tween.to = new Vector3(enable ? 1.0f : 0.0f,0.0f,0.0f);
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}

		public static BETween anchoredPosition3D(GameObject go, float _duration, Vector3 _from, Vector3 _pos) {
			BETween tween = add(go, _duration, BETweenType.anchoredPosition3D);
			tween.from = _from;
			tween.to = _pos;
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween anchoredPosition3D(GameObject go, float _duration, Vector3 _pos) {
			BETween tween = add(go, _duration, BETweenType.anchoredPosition3D);
			tween.from = tween.rt.anchoredPosition;
			tween.to = _pos;
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}
		
		public static BETween anchoredPosition(GameObject go, float _duration, Vector2 _from, Vector2 _pos) {
			BETween tween = add(go, _duration, BETweenType.anchoredPosition);
			tween.from = new Vector3(_from.x,_from.y,0);
			tween.to = new Vector3(_pos.x,_pos.y,0);
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween anchoredPosition(GameObject go, float _duration, Vector2 _pos) {
			BETween tween = add(go, _duration, BETweenType.anchoredPosition);
			tween.from = tween.rt.anchoredPosition;
			tween.to = new Vector3(_pos.x,_pos.y,0);
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}
		
		public static BETween size(GameObject go, float _duration, Vector2 _from, Vector2 _scale) {
			BETween tween = add(go, _duration, BETweenType.size);
			tween.from = new Vector3(_from.x, _from.y, 0);
			tween.to = new Vector3(_scale.x, _scale.y, 0);
			tween.current = tween.from;
			tween.applyFrom = true;
			return tween;
		}
		public static BETween size(GameObject go, float _duration, Vector2 _scale) {
			BETween tween = add(go, _duration, BETweenType.size);
			tween.from = tween.rt.localScale;
			tween.to = new Vector3(_scale.x, _scale.y, 0);
			tween.current = tween.from;
			tween.applyFrom = false;
			return tween;
		}

	}
}