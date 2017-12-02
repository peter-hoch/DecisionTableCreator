/*
 * [The "BSD license"]
 * Copyright (c) 2017 Peter Hoch
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DecisionTableCreator.Properties;

namespace DecisionTableCreator.Utils
{

    public class WindowPosition
    {

        [SaveValue]
        public double Top { get; set; }
        [SaveValue]
        public double Left { get; set; }
        [SaveValue]
        public double Height { get; set; }
        [SaveValue]
        public double Width { get; set; }
        [SaveValue]
        public WindowState WindowState { get; set; }
        [SaveValue]
        public bool IsValid { get; set; }

        public string Name { get; private set; }

        public WindowPosition(string name)
        {
            IsValid = false;
            Name = name;
        }

        public void SetWindowPosition(Window window)
        {
            if (IsValid)
            {
                if (SystemParameters.VirtualScreenLeft < Left &&
                    SystemParameters.VirtualScreenTop < Top &&
                    SystemParameters.VirtualScreenWidth > Width &&
                    SystemParameters.VirtualScreenHeight > Height)
                {
                    if (WindowState == WindowState.Normal || WindowState == WindowState.Maximized)
                    {
                        window.WindowState = WindowState;
                        window.Top = Top;
                        window.Left = Left;
                        window.Height = Height;
                        window.Width = Width;
                    }
                }
            }
        }

        public void GetWindowPosition(Window window)
        {
            if (window.WindowState == WindowState.Normal || window.WindowState == WindowState.Maximized)
            {
                IsValid = true;
                WindowState = window.WindowState;
            }
            else if (window.WindowState == WindowState.Minimized)
            {
                IsValid = true;
                WindowState = WindowState.Normal;
            }
            Top = window.Top;
            Left = window.Left;
            Height = window.Height;
            Width = window.Width;
        }

        internal void Save(Settings defaultSetting)
        {
            List<PropertyInfo> sprops = GetSavedProperties();
            foreach (PropertyInfo info in sprops)
            {
                string value = Convert.ToString(info.GetValue(this), CultureInfo.InvariantCulture);
                string name = Name + info.Name;
                PropertyInfo prop = defaultSetting.GetType().GetProperties().Where(p => p.Name.Equals(name)).FirstOrDefault();
                prop.SetValue(defaultSetting, value);
            }
        }

        internal void Load(Settings defaultSetting)
        {
            try
            {
                List<PropertyInfo> sprops = GetSavedProperties();
                foreach (PropertyInfo info in sprops)
                {
                    string name = Name + info.Name;
                    PropertyInfo prop = defaultSetting.GetType().GetProperties().Where(p => p.Name.Equals(name)).FirstOrDefault();
                    string value = prop.GetValue(defaultSetting).ToString();
                    object convertedBValue = ConvertValue(value, info.PropertyType);
                    info.SetValue(this, convertedBValue);
                }
            }
            catch (Exception)
            {
                IsValid = false;
            }
        }

        public object ConvertValue(string value, Type type)
        {
            if (type.GetTypeInfo().IsEnum)
            {
                return Enum.Parse(type, value);
            }

            return System.Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        private List<PropertyInfo> GetSavedProperties()
        {
            List<PropertyInfo> saveProperties = new List<PropertyInfo>();
            var localProps = GetType().GetProperties();
            foreach (PropertyInfo info in localProps)
            {
                if (info.GetCustomAttributes(typeof(SaveValueAttribute)).Count() != 0)
                {
                    saveProperties.Add(info);
                }
            }
            return saveProperties;
        }
    }
}
