﻿using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
   public class StatUI : MonoBehaviour
   {
      [SerializeField] private Text title;
      [SerializeField] private Image image;

      private Stat _stat = new Stat();
      private Buff _buff = new Buff();

      public Stat GetStat()
      {
         return _stat;
      }

      public Buff GetBuff()
      {
         return _buff;
      }
      
      public void Initialize(Stat stat)
      {
         _stat = stat;
         
         title.text = stat.value.ToString(CultureInfo.InvariantCulture);
         image.sprite = Resources.Load<Sprite>($"Icons/{stat.icon}");
      }
      
      public void Initialize(Buff buff)
      {
         _buff = buff;

         title.text = buff.title;
         image.sprite = Resources.Load<Sprite>($"Icons/{buff.icon}");
      }

      public void AddStatValue(float value)
      {
         _stat.value += value;
         title.text = _stat.value.ToString(CultureInfo.InvariantCulture);
      }

      public void UpdateStatValue(float value)
      {
         title.text = value.ToString(CultureInfo.InvariantCulture);
      }
   }
}
