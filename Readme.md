#Decesion Table Creater
##Main features


- Define conditions with more than 2 states
- Define don't care entries in conditions 
- Customizable code generator
- Calculation of possible combinations and coverage

![](./MainWindow.png)

##Menu description

###File - New
Create a new project
###File - Open
Open an existing project
###File - Save / Save as
Save the current project
###File - Create sample project
Create the printer trubbleshooting sample
###File - Exit
Exit tool

###Edit - Append test case / Delete test case
Add and delete test case
###Edit - Edit condition / Edit action (or double click on condition or action name)  
edit conditioon or action  
###Edit - Append condition / Insert condition 
Add or insert new condition - the edit condition or action dilog box appears
###Edit - Delete action / Delete condition
Delete condition or action - the condition or action must be selected
###Edit - Move up / Move down
Move condition or action one line up or down - the condition or action you want to move must be selected

###Export - Export HTML to clipboard
copy the current decision table as HTML to clipbord
use this to export the table to a word processing or spreadsheet tool 
###Export - External template - Sample.file.stg
Code generation access
the templates are locaten in directory "MyDocuments"/DecisionTableCreatorTemplates/*.stg
The first template "Sample.file.stg" is written to this directory during first start of the tool. This is the place to store other templates. During the start of the tool a submenu entry is created for every template in this directory. A template must have the extension "stg".

##Code generation
The code generation is based on StringTemplate [https://github.com/antlr/stringtemplate4](https://github.com/antlr/stringtemplate4)

Further documentation: 
[StringTemplate cheat sheet](https://github.com/antlr/stringtemplate4/blob/master/doc/cheatsheet.md "https://github.com/antlr/stringtemplate4/blob/master/doc/cheatsheet.md")

