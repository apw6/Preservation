﻿using UnityEngine;
using System.Collections;

public class GenericAnimalBehavior : MonoBehaviour {

   float mCurrentScanValue;
   const float mTargetScanValue = 100;
   const float mScanRate = 50;//scan points per second

   //scan progresss bar
   Vector2 scanBarPos;
   Vector2 scanBarSize;
   Texture2D blackPixel;
   Texture2D barEmpty;
   Texture2D barFull;
   const float mScanBarOffsetY = .75f; //offset from the objects position in y direction

	// Use this for initialization
	void Start () {
      mCurrentScanValue = 0;
      scanBarSize = new Vector2(50, 10);

      barEmpty = new Texture2D(1, 1);
      Color [] pixels = barEmpty.GetPixels();
      for(int i = 0; i < pixels.Length; i++) {
         pixels[i] = new Color(255,255,255,255);
      }
      barEmpty.SetPixels(pixels);
      barEmpty.Apply();

      blackPixel = new Texture2D(1, 1);
      pixels = blackPixel.GetPixels();
      for (int i = 0; i < pixels.Length; i++) {
         pixels[i] = new Color(0, 0, 0, 255);
      }
      blackPixel.SetPixels(pixels);
      blackPixel.Apply();

      barFull = new Texture2D(1, 1);
      pixels = barFull.GetPixels();
      for (int i = 0; i < pixels.Length; i++) {
         pixels[i] = Color.blue;
      }
      barFull.SetPixels(pixels);
      barFull.Apply();

   }
	
	// Update is called once per frame
	void Update () {
	   if(mCurrentScanValue >= mTargetScanValue) {
			CameraScript globalBehavior = GameObject.Find ("Main Camera").GetComponent<CameraScript>();
			globalBehavior.animalCount++;
         	Destroy(gameObject);
      }

      //update position of scan progress bar based on the animal's position
      Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y + mScanBarOffsetY);
      scanBarPos = (Vector2)Camera.main.WorldToScreenPoint(currentPosition);
      scanBarPos.y = Screen.height - scanBarPos.y;
      scanBarPos.x -= scanBarSize.x / 2;
	}

   public void scanned() {
      scanned(1);
   }
   public void scanned(float multiplier) {
      mCurrentScanValue += mScanRate * multiplier * Time.deltaTime;
      if(mCurrentScanValue > mTargetScanValue) {
         mCurrentScanValue = mTargetScanValue;
      }
   }

   void OnGUI() {
      GUI.BeginGroup(new Rect(scanBarPos.x, scanBarPos.y, scanBarSize.x, scanBarSize.y));
      GUI.DrawTexture(new Rect(0, 0, scanBarSize.x, scanBarSize.y), blackPixel, ScaleMode.StretchToFill);


      GUI.BeginGroup(new Rect(1, 2, scanBarSize.x-2, scanBarSize.y -4));
      GUI.DrawTexture(new Rect(0, 0, scanBarSize.x, scanBarSize.y), barEmpty, ScaleMode.StretchToFill);

      //draw the filled-in part:
      GUI.BeginGroup(new Rect(0, 0, scanBarSize.x * (mCurrentScanValue / mTargetScanValue), scanBarSize.y));
      GUI.DrawTexture(new Rect(0, 0, scanBarSize.x, scanBarSize.y), barFull, ScaleMode.StretchToFill);
      GUI.EndGroup();
      GUI.EndGroup();
      GUI.EndGroup();
   }
}
