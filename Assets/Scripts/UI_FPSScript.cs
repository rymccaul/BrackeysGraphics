using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_FPSScript : MonoBehaviour
{
  public float updateInterval = 0.5F;
  private double lastInterval;
  private int frames = 0;
  private float fps;
  private TextMeshProUGUI FPS_text_obj;

  void Start()
  {
      FPS_text_obj = GameObject.Find("UI_FPS").GetComponent<TMPro.TextMeshProUGUI>();
      lastInterval = Time.realtimeSinceStartup;
      frames = 0;
  }

  void Update()
  {
      ++frames;
      float timeNow = Time.realtimeSinceStartup;
      if (timeNow > lastInterval + updateInterval)
      {
          fps = (float)(frames / (timeNow - lastInterval));
          frames = 0;
          lastInterval = timeNow;
          FPS_text_obj.text = "FPS | " + fps.ToString();
      }
  }
}
