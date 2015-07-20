﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 12.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Data_Loading_Tool.Templates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public partial class InsertMeasureTemplate : InsertMeasureTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\nINSERT INTO Fact\r\n(\r\n\tFactName\r\n)\r\nVALUES\r\n(\r\n\t\'");
            
            #line 14 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FactName));
            
            #line default
            #line hidden
            this.Write("\'\r\n)\r\n\r\nDeclare @FactID int\r\nSelect\r\n\t@FactID = FactID\r\nfrom\r\n\tFact\r\nwhere\r\n\tFact" +
                    "Name = \'");
            
            #line 23 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FactName));
            
            #line default
            #line hidden
            this.Write("\'\r\n\r\n\r\n\r\n\r\n\r\nINSERT INTO FactDimensionSet\r\n(\r\n\tFactID,\r\n\tDimString\r\n)\r\n\tselect @F" +
                    "actID, \r\n");
            
            #line 35 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
 
	bool firstSelect = true;

	foreach(int id in DimensionIDs)
	{
		if(firstSelect)
		{

            
            #line default
            #line hidden
            this.Write("\t[");
            
            #line 43 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("].DimensionValue\r\n");
            
            #line 44 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"

		firstSelect = false;
		}
		else
		{

            
            #line default
            #line hidden
            this.Write("\t+ \'-\' + [");
            
            #line 50 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("].DimensionValue\r\n");
            
            #line 51 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"

		}
	}

	bool first = true;
	int firstID = 0;

	foreach(int id in DimensionIDs)
	{
	if(first)
	{

            
            #line default
            #line hidden
            this.Write("from\r\n(select \r\n\tV.DimensionID as ID,\r\n\tV.DimensionValue\r\nfrom\r\n\tDimension D\r\ninn" +
                    "er join \r\n\tDimensionValue V\r\non D.DimensionID = v.DimensionID\r\nwhere D.Dimension" +
                    "ID = ");
            
            #line 72 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write(") [");
            
            #line 72 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("]\r\n\r\n");
            
            #line 74 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"

first = false;
firstID = id; 
}
else{

            
            #line default
            #line hidden
            this.Write("inner join \r\n(select \r\n\tV.DimensionID as ID,\r\n\tV.DimensionValue\r\nfrom\r\n\tDimension" +
                    " D\r\ninner join \r\n\tDimensionValue V\r\non D.DimensionID = v.DimensionID\r\nwhere D.Di" +
                    "mensionID = ");
            
            #line 89 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write(") [");
            
            #line 89 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("]\r\non [");
            
            #line 90 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(firstID));
            
            #line default
            #line hidden
            this.Write("].ID <> [");
            
            #line 90 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("].ID\r\n");
            
            #line 91 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"

}
}

            
            #line default
            #line hidden
            this.Write("\r\n\r\n\r\n\r\n");
            
            #line 99 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
 
	foreach(int dimension in DimensionIDs)
	{

            
            #line default
            #line hidden
            this.Write("\tINSERT INTO FactDimensionMapping\r\n\t(\r\n\t\tDimensionValueID,\r\n\t\tFactDimensionSetID\r" +
                    "\n\t)\r\n\tselect \r\n\t\tXXX.ID, F.FactDimensionSetID\r\n\tfrom \r\n\t(\r\n\tselect [");
            
            #line 112 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(dimension));
            
            #line default
            #line hidden
            this.Write("].ID , \r\n");
            
            #line 113 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
 
	bool firstSelect2 = true;

	foreach(int id in DimensionIDs)
	{
		if(firstSelect2)
		{

            
            #line default
            #line hidden
            this.Write("\t[");
            
            #line 121 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("].DimensionValue\r\n");
            
            #line 122 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"

		firstSelect2 = false;
		}
		else
		{

            
            #line default
            #line hidden
            this.Write("\t+ \'-\' + [");
            
            #line 128 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("].DimensionValue\r\n");
            
            #line 129 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"

		}
	}

            
            #line default
            #line hidden
            this.Write("\tas string\r\n\tfrom\r\n\r\n");
            
            #line 136 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"


	bool first2 = true;
	int firstID2 = 0;

	foreach(int id in DimensionIDs)
	{
		if(first2)
		{


            
            #line default
            #line hidden
            this.Write("\t\t(select \r\n\t\t\tV.DimensionValueID as ID,\r\n\t\t\tV.DimensionValue\r\n\t\tfrom\r\n\t\t\tDimensi" +
                    "on D\r\n\t\tinner join \r\n\t\t\tDimensionValue V\r\n\t\ton D.DimensionID = v.DimensionID\r\n\t\t" +
                    "where D.DimensionID = ");
            
            #line 155 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write(") [");
            
            #line 155 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("]\r\n\r\n");
            
            #line 157 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"

		first2 = false;
		firstID2 = id; 
		}
		else
		{

            
            #line default
            #line hidden
            this.Write("\t\tinner join \r\n\t\t(select \r\n\t\t\tV.DimensionValueID as ID,\r\n\t\t\tV.DimensionValue\r\n\t\tf" +
                    "rom\r\n\t\t\tDimension D\r\n\t\tinner join \r\n\t\t\tDimensionValue V\r\n\t\ton D.DimensionID = v." +
                    "DimensionID\r\n\t\twhere D.DimensionID = ");
            
            #line 173 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write(") [");
            
            #line 173 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("]\r\n\t\ton [");
            
            #line 174 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(firstID2));
            
            #line default
            #line hidden
            this.Write("].ID <> [");
            
            #line 174 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(id));
            
            #line default
            #line hidden
            this.Write("].ID\r\n");
            
            #line 175 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"

		}
	}

            
            #line default
            #line hidden
            this.Write("\r\n\t) XXX\r\n\tinner join FactDimensionSet F\r\n\ton F.DimString = XXX.string\r\n");
            
            #line 183 "C:\Development\Ben B\Code\Geographical Needs\Data Loading Tool\Templates\InsertMeasureTemplate.tt"


	}


            
            #line default
            #line hidden
            this.Write("\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public class InsertMeasureTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}