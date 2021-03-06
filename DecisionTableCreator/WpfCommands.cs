﻿/*
 * [The "BSD license"]
 * Copyright (c) 2016 Peter Hoch
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DecisionTableCreator
{
    public class WpfCommands
    {
        public static readonly RoutedUICommand AppendTestCase;
        public static readonly RoutedUICommand DeleteTestCase;
        public static readonly RoutedUICommand DeleteMostRightTestCase;
        public static readonly RoutedUICommand InsertTestCase;
        public static readonly RoutedUICommand EditCondition;
        public static readonly RoutedUICommand DeleteCondition;
        public static readonly RoutedUICommand DeleteBottomCondition;
        public static readonly RoutedUICommand EditAction;
        public static readonly RoutedUICommand EditConditionOrAction;
        public static readonly RoutedUICommand DeleteAction;
        public static readonly RoutedUICommand Save;
        public static readonly RoutedUICommand SaveAs;
        public static readonly RoutedUICommand Open;
        public static readonly RoutedUICommand NewDocument;
        public static readonly RoutedUICommand AppendCondition;
        public static readonly RoutedUICommand InsertCondition;
        public static readonly RoutedUICommand AppendAction;
        public static readonly RoutedUICommand InsertAction;
        public static readonly RoutedUICommand AppendEnumValue;
        public static readonly RoutedUICommand DeleteEnumValue;
        public static readonly RoutedUICommand ExportToClipboard;
        public static readonly RoutedUICommand MoveConditionOrActionUp;
        public static readonly RoutedUICommand MoveConditionOrActionDown;
        public static readonly RoutedUICommand ExportToFileWithExternalTemplate;
        public static readonly RoutedUICommand CreateSampleProject;
        public static readonly RoutedUICommand Exit;
        public static readonly RoutedUICommand ExportToClipboardWithExternalTemplate;
        public static readonly RoutedUICommand OpenTemplateDirectory;
        public static readonly RoutedUICommand CreateMissingTestCases;
        public static readonly RoutedUICommand EditTestCaseDescription;
        public static readonly RoutedUICommand OpenReadMeFile;
        public static readonly RoutedUICommand AppendCopyOfTestCase;

        static WpfCommands()
        {
            KeyGesture gestureAltC = new KeyGesture(Key.C, ModifierKeys.Alt);
            KeyGesture gestureAltA = new KeyGesture(Key.A, ModifierKeys.Alt);
            KeyGesture gestureAltT = new KeyGesture(Key.T, ModifierKeys.Alt);
            KeyGesture gestureAltE = new KeyGesture(Key.E, ModifierKeys.Alt);
            KeyGesture gestureCtrlS = new KeyGesture(Key.S, ModifierKeys.Control);
            AppendTestCase = new RoutedUICommand("Append test case", "AppendTestCase", typeof(WpfCommands), new InputGestureCollection() { gestureAltT });
            DeleteTestCase = new RoutedUICommand("Delete test case", "DeleteTestCase", typeof(WpfCommands));
            DeleteMostRightTestCase = new RoutedUICommand("Delete most right test case", "DeleteMostRightTestCase", typeof(WpfCommands));
            InsertTestCase = new RoutedUICommand("Insert test case", "InsertTestCase", typeof(WpfCommands));
            EditCondition = new RoutedUICommand("Edit condition", "EditCondition", typeof(WpfCommands));
            EditConditionOrAction = new RoutedUICommand("Edit condition or action", "EditConditionOrAction", typeof(WpfCommands));
            DeleteCondition = new RoutedUICommand("Delete condition", "DeleteCondition", typeof(WpfCommands));
            DeleteBottomCondition = new RoutedUICommand("Delete bottom condition", "DeleteBottomCondition", typeof(WpfCommands));
            EditAction = new RoutedUICommand("Edit action", "EditAction", typeof(WpfCommands));
            DeleteAction = new RoutedUICommand("Delete action", "DeleteAction", typeof(WpfCommands));
            Save = new RoutedUICommand("Save", "Save", typeof(WpfCommands), new InputGestureCollection() { gestureCtrlS });
            SaveAs = new RoutedUICommand("Save as", "SaveAs", typeof(WpfCommands));
            Open = new RoutedUICommand("Open", "Open", typeof(WpfCommands));
            NewDocument = new RoutedUICommand("New", "NewDocument", typeof(WpfCommands));
            AppendCondition = new RoutedUICommand("Append condition", "AppendCondition", typeof(WpfCommands), new InputGestureCollection() { gestureAltC});
            InsertCondition = new RoutedUICommand("Insert condition", "InsertCondition", typeof(WpfCommands));
            AppendAction = new RoutedUICommand("Append action", "AppendAction", typeof(WpfCommands), new InputGestureCollection() { gestureAltA });
            InsertAction = new RoutedUICommand("Insert action", "InsertAction", typeof(WpfCommands));
            AppendEnumValue = new RoutedUICommand("Append enum value", "AppendEnumValue", typeof(WpfCommands), new InputGestureCollection() { gestureAltE });
            DeleteEnumValue = new RoutedUICommand("Delete enum value", "DeleteEnumValue", typeof(WpfCommands));
            ExportToClipboard = new RoutedUICommand("Export to clipboard", "ExportToClipboard", typeof(WpfCommands));
            MoveConditionOrActionUp = new RoutedUICommand("Move up", "MoveConditionOrActionUp", typeof(WpfCommands));
            MoveConditionOrActionDown = new RoutedUICommand("Move down", "MoveConditionOrActionDown", typeof(WpfCommands));
            ExportToFileWithExternalTemplate = new RoutedUICommand("Export to file with external template", "ExportToFileWithExternalTemplate", typeof(WpfCommands));
            CreateSampleProject = new RoutedUICommand("Create sample project", "CreateSampleProject", typeof(WpfCommands));
            Exit = new RoutedUICommand("Exit", "Exit", typeof(WpfCommands));
            ExportToClipboardWithExternalTemplate = new RoutedUICommand("ExportToClipboardWithExternalTemplate", "ExportToClipboardWithExternalTemplate", typeof(WpfCommands));
            OpenTemplateDirectory = new RoutedUICommand("Open template directory", "OpenTemplateDirectory", typeof(WpfCommands));
            CreateMissingTestCases = new RoutedUICommand("Create missing testcases", "CreateMissingTestCases", typeof(WpfCommands));
            EditTestCaseDescription = new RoutedUICommand("Edit test case description", "EditTestCaseDescription", typeof(WpfCommands));
            OpenReadMeFile = new RoutedUICommand("Open ReadMe", "OpenReadMeFile", typeof(WpfCommands));
            AppendCopyOfTestCase = new RoutedUICommand("Append a copy of the selected test case", "AppendCopyOfTestCase", typeof(WpfCommands));
        }
    }
}
