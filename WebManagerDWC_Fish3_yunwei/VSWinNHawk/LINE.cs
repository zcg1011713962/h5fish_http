/* 
Copyright (C) 2008 Michael Corley  
All rights reserved.

Copyright (c) 1997-2008 Tobias Oetiker
All rights reserved.

                                            
NHAWK GPL License Info.
======================

This file is part of the NHAWK project.
NHAWK is free software; you can redistribute it and/or modify                        
it under the terms of the GNU General Public License as published by                 
the Free Software Foundation; either version 3 of the License, or                    
(at your option) any later version.                                                  
                                                                                         
NHAWK is distributed in the hope that it will be useful,                             
but WITHOUT ANY WARRANTY; without even the implied warranty of                       
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                        
GNU General Public License for more details.                                         
                                                                                         
You should have received a copy of the GNU General Public License                    
along with NHAWK; if not, write to the Free Software Foundation, Inc.,               
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA  
                           
                                                                                        
RRDTool GPL License Info.
========================

RRDTool is free software; you can redistribute it and/or modify it
under the terms of the GNU General Public License as published by the Free
Software Foundation; either version 2 of the License, or (at your option)
any later version.

RRDTool is distributed in the hope that it will be useful, but WITHOUT
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
more details.

You should have received a copy of the GNU General Public License along
with this RRDTool; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA

FLOSS License Exception 
=======================
(Adapted from http://www.mysql.com/company/legal/licensing/foss-exception.html)

I want specified Free/Libre and Open Source Software ("FLOSS")
applications to be able to use specified GPL-licensed RRDtool
libraries (the "Program") despite the fact that not all FLOSS licenses are
compatible with version 2 of the GNU General Public License (the "GPL").

As a special exception to the terms and conditions of version 2.0 of the GPL:

You are free to distribute a Derivative Work that is formed entirely from
the Program and one or more works (each, a "FLOSS Work") licensed under one
or more of the licenses listed below, as long as:

1. You obey the GPL in all respects for the Program and the Derivative
Work, except for identifiable sections of the Derivative Work which are
not derived from the Program, and which can reasonably be considered
independent and separate works in themselves,

2. all identifiable sections of the Derivative Work which are not derived
from the Program, and which can reasonably be considered independent and
separate works in themselves,

1. are distributed subject to one of the FLOSS licenses listed
below, and

2. the object code or executable form of those sections are
accompanied by the complete corresponding machine-readable source
code for those sections on the same medium and under the same FLOSS
license as the corresponding object code or executable forms of
those sections, and

3. any works which are aggregated with the Program or with a Derivative
Work on a volume of a storage or distribution medium in accordance with
the GPL, can reasonably be considered independent and separate works in
themselves which are not derivatives of either the Program, a Derivative
Work or a FLOSS Work.

If the above conditions are not met, then the Program may only be copied,
modified, distributed or used under the terms and conditions of the GPL.

FLOSS License List
==================
License name	Version(s)/Copyright Date
Academic Free License		2.0
Apache Software License	1.0/1.1/2.0
Apple Public Source License	2.0
Artistic license		From Perl 5.8.0
BSD license			"July 22 1999"
Common Public License		1.0
GNU Library or "Lesser" General Public License (LGPL)	2.0/2.1
IBM Public License, Version    1.0
Jabber Open Source License	1.0
MIT License (As listed in file MIT-License.txt)	-
Mozilla Public License (MPL)	1.0/1.1
Open Software License		2.0
OpenSSL license (with original SSLeay license)	"2003" ("1998")
PHP License			3.0
Python license (CNRI Python License)	-
Python Software Foundation License	2.1.1
Sleepycat License		"1999"
W3C License			"2001"
X11 License			"2001"
Zlib/libpng License		-
Zope Public License		2.0/2.1
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//File       : LINE.cs   -- .Net / Mono graph element LINE class                                              //
//Application: NHawk: Open Source Project Support                                                             // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                                //
//Author     : Mike Corley, Syracuse University                                                               //
//             mwcorley@syr.edu                                                                               //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides the class definition for the rrdtool LINE construct.  LINE constitutes a thin wrapper
 * around the rrdtool LINE graph element construct. It is intended that the public interface for this class 
 * intuitively resemble native rrdtool construct syntax. For usage detail public interface below, rrdtool man page 
 * documentation at: http://oss.oetiker.ch/rrdtool/doc/rrdgraph_graph.en.html.
 * 
 * Note about this class and the NHAWK project: This class seeks to encapsulate rrdtool constructs the in C# .Net / Mono 
 * object model while retaining intuitive use of the RRDTool syntax. It also seeks to enforce correct usage of rrdtool
 * constructs by providing semantically appropriate exception handling.  The primary goal of the NHAWK project is to enable
 * .Net developers to use RRDTool directly from C#, while retaining the native (intuitive use) of rrdtool construct sytnax.  
 * It is our intent that NHAWK constitute a very thin rrdtool provider layer, enabling proficient .Net developers to rapidly 
 * gain productivity by referring directly (and intuitively) to the main rrdtool documentation page at: 
 * http://oss.oetiker.ch/rrdtool/doc/rrdgraph.en.html
 *
 *
 * Public Interface
 * ================  
 * [Serializable]
 * public class LINE : AGRAPH_ELEMENT  <-- derived class AREA
 * 
 * public LINE(string val, object set_to_null)
 * -- constructor:  accepts val (the basic def). e.g LINE:[val][variable],
 *                               note: this line will appear invisible because no color was or width was specified
 *                  val        : can be a static value, or a valid variable as defined by a DEF or CDEF 
 *                  set_to_null: should always be set to null (this is just place holder and has no 
 *                               significance other than to provide a unique function prototype\definition,
 *                               otherwise it conflicts with de-serialization ctor: "public LINE(string elem)" )
 *  
 * public LINE(string val, Color c)
 *  -- constructor: accepts val and color e.g LINE:[1000][variable]#color
 *     val        : can be a static value, or a valid variable as defined by a DEF or CDEF 
 *     color      : the color of visualized area
 * 
 * public LINE(double width, string val, Color c)
 *  -- constructor: accepts width, val, color, and legend   e.g LINE:1000#color:"this a a legend"
 *     width      : the width of the line 
 *     val        : can be a static value or valid variable as defined by a DEF or CDEF 
 *     color      : the color of the visualized area
 *     legend     : the text label associated the element 
 *                  note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                        a GraphFormatException will be thrown
 * 
 * public LINE(double width, string val, Color c, string legend, bool stack)
 * -- constructor: accepts width, val_name, color, legend, and stack     e.g LINE:1000#color:"this a a legend":STACK
 *    width      : the width of the line 
 *    val        : can be a static value or valid variable as defined by a DEF or CDEF 
 *    color      : the color of the visualized area
 *    legend     : the text label associated the element 
 *                  note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                        a GraphFormatException will be thrown
 *    stack      : a boolean which determines whether or not this element is stacked above
 *                 the previously defined graph element
 *                 
 * public LINE(double width, string val, Color c, string legend, bool stack, bool dashes_val)
 * -- constructor: accepts width, val, color, legend, stack, dashes_val e.g LINE:1000#color:"this a a legend":STACK:dashes
 *    width      : the width of the line 
 *    val        : can be a static value or valid variable as defined by a DEF or CDEF 
 *    color      : the color of the visualized area
 *    legend     : the text label associated the element 
 *                 note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                       a GraphFormatException will be thrown
 *    stack      : a boolean which determines whether or not this element is stacked above
 *                 the previously defined graph element
 *    dashes_val : if set to true, the dashes option will be set causing the LINE element to be displayed with dashes
 * 
 * public LINE(double width, string val, Color c, string legend, bool stack, int[] dashes_val)
 * -- constructor: accepts width, val, color, legend, stack, dashes_val e.g LINE:1000#color:"this a a legend":STACK:dashes=10,5,5,10
 *    width      : the width of the line 
 *    val        : can be a static value or valid variable as defined by a DEF or CDEF 
 *    color      : the color of the visualized area
 *    legend     : the text label associated the element 
 *                 note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                       a GraphFormatException will be thrown
 *    stack      : a boolean which determines whether or not this element is stacked above
 *                 the previously defined graph element
 *    dashes_val : array of integer parameters, that affact appearance and dash thickness (see rrdtool docs)
 *    
 * public LINE(double width, string def_name, Color c, string legend, bool stack, int[] dashes_val, int dashes_offset)
 * -- constructor: accepts width, val_name, color, legend, stack, dashes_val e.g LINE:1000#color:"this a a legend":STACK:dashes=10,5,5,10:20
 *    width        : the width of the line 
 *    val          : can be a static value or valid variable as defined by a DEF or CDEF 
 *    color        : the color of the visualized area
 *    legend       : the text label associated the element 
 *                   note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                       a GraphFormatException will be thrown
 *    stack        : a boolean which determines whether or not this element is stacked above
 *                   the previously defined graph element
 *    dashes_val   : array of integer parameters, that affact appearance and dash thickness of the line (see rrdtool docs)
 *    dashes_offset: determines the value at which the dashed line begins
 *  
 * public LINE(string element)
 *   -- deserialization constructor:  accepts a raw rrdtool level LINE element construct as a string and deserializes it resulting
 *                                    in a fully constructed AREA object.  The object can then be manipulated and serialized once 
 *                                    again by calling the ToString() override.  The is used primary by the GRAPH class which will
 *                                    ask each graph element (contained within its definition) to deserialize itself.
 *   
 * public override string ToString()
 *   -- serialization override:  serializes the LINE element to a string.  The is called polymorphically by the GRAPH class 
 *                               which will ask each graph element (contained within its definition) to serialize itself so the entire
 *                               GRAPH structure (as a composite structure), can be serialized and sent to rrdtool.exe for execution.
 *                               
 * public override GRAPH_ELEMENT DeepClone()
 *   -- deep clone override: polymorphically returns a (new memory) copy of the LINE element. This is used by the GRAPH class for adding
 *                           elements to the element list or tracking.  
 *                           Note on DeepCloning: In the NHAWK model, the DeepClone works in the shallow reference model because every member
 *                           member stored is either value type or string reference type and new string objects are construced, initialized and 
 *                           returned with temppory StringBuilder(), with it's ToString() method is called to yield the new string instance.
 *                           This is useful be graph definitions won't be susceptible to potential modification by external defined references.                           
 * 
 * public static LINE DeepClone(AREA elem)
 *    -- provides a the same operation as the method above, but doesn't work polymorphically.  This is generally the 
 *       the version that would be called by clients.
 *       
 * public void set_dashes(int[] dashes_args)
 *    -- array of integer parameters, that affact appearance and dash thickness of the line (see rrdtool docs)
 *       e.g LINE:1000#color:"this a a legend":STACK:dashes=10,5,5,10       
 *       
 * public int dash_offset
 *    -- gets or sets value at which the dashed line begins
 *       e.g LINE:1000#color:"this a a legend":STACK:dashes=10,5,5,10:20
 *      
 * public TYPE type
 *   -- gets or the element type: LINE
 *     
 * public Color color
 *   -- gets or sets the color of the visualizable element
 *      e.g. LINE:1000#color:"this a a legend"
 *    
 * public string val
 *   -- gets or sets the graph definition variable or value.  
 *      val must consist of a contiguous sequence of characters (no spaces) 
 *      or GraphElementFormatException will be thrown.
 *      e.g. LINE:val
 *   
 * public bool stack
 *   -- gets or sets a boolean which determines whether or not this element is stacked above
 *      the previously defined graph element
 *      LINE:1000#color:"this a a legend"
 *         or
 *      LINE:1000#color:"this a a legend":STACK
 *      
 * public string legend
 *   -- gets or sets the legend. Note:  if specified when color == Color.IsEmpty == true, and
 *      a GraphFormatException will be thrown.
 *      LINE:1000#color:"this a a legend"  
 * 
 * Example Usage
 * =============
 * See test stub below
 * 
 * 
 *  Example Usage
 *  =============
 *  See test stub below
 * 
 *  This module can be compiled stand alone for testing purposes: 
 *  Note: some modules have additional assemblies dependencies which
 *  are needed for compilation of the test stub.  The required files section
 *  shows true dependency structure. i.e when the test stub is not compiled.      
 *
 *  Compiler Command:
 *  Visual Studio 2008 (.Net 3.5): csc /define:TEST_LINE_ELEMENT   LINE.cs  /reference:c:\windows\microsoft.net\framework\v2.0.50727\Microsoft.VisualBasic.dll 
 *                                                                 ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs
 *    
 *  Win32 Mono (1.9.1)           : gmcs -define:TEST_LINE_ELEMENT -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.Drawing.dll 
 *                                                                -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\Microsoft.VisualBasic.dll
 *                                                                -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.dll ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs LINE.cs
 *                                                                   
 *  SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_LINE_ELEMENT -r:/usr/lib/mono/gac/System/2.0.0.0__b77a5c561934e089/System.dll 
 *                                                                -r:/usr/lib/mono/gac/System.Drawing/2.0.0.0__b03f5f7f11d50a3a/System.Drawing.dll 
 *                                                                -r:/usr/lib/mono/gac/Microsoft.VisualBasic/8.0.0.0__b03f5f7f11d50a3a/Microsoft.VisualBasic.dll  
 *                                                                ../GRAPH_ELEMENT/GRAPH_ELEMENT.cs LINE.cs                                              
 *                                                                  
 *  Required Files:   LINE.cs GRAPH_ELEMENT.cs
 *
 *
 * Maintanence History
 * ===================
 * version 1.0 : 01 August 08 
 *     -- first release: 
 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NHAWK
{
    [Serializable]
    public class LINE : AGRAPH_ELEMENT
    {
        private double width_;
       

        private LINE()
            : base(TYPE.LINE, "")
        {
            this.width = 0;
        }

        public LINE(string def_name, object set_to_null)
            : base(TYPE.LINE, def_name)
        {
            this.width = 0;
        } 

        public LINE(string def_name, Color c)
            : base(TYPE.LINE, def_name, c)
        {
            this.width = 0;
        }

        public LINE(double width, string def_name, Color c)
            : base(TYPE.LINE, def_name, c)
        {
            this.width = width;
        }

        public LINE(double width, string def_name, Color c, string legend, bool stack)
            : base(TYPE.LINE, def_name, c, legend, stack)
        {
            this.width = width;
        }

        public LINE(double width, string def_name, Color c, string legend, bool stack, bool dashes_val)
            : base(TYPE.LINE, def_name, c, legend, stack, dashes_val)
        {
            this.width = width;
        }

        public LINE(double width, string def_name, Color c, string legend, bool stack, int[] dashes_val)
            : base(TYPE.LINE, def_name, c, legend, stack, dashes_val)
        {
            this.width = width;
        }

        public LINE(double width, string def_name, Color c, string legend, bool stack, int[] dashes_val, int dashes_offset)
            : base(TYPE.LINE, def_name, c, legend, stack, dashes_val, dashes_offset)
        {
            this.width = width;
        }


        public LINE(string element): this()
        {
            try
            {
                //parse out all tokens (separated by ':' )
                string[] elem_tokens = element.Split(new char[] { ':' });

                //check that the minumum number of tokens exist
                if (elem_tokens.Length < MIN_TOKEN_COUNT)
                    throw new GraphElementFormatException("bad LINE element format");

                //determine whether the width attribute was specified
                if (elem_tokens[0].Length > "LINE".Length)
                {
                    string w = elem_tokens[0].Substring("LINE".Length, (elem_tokens[0].Length - "LINE".Length));
                    width = double.Parse(w);
                    myset_type = StrToElem(elem_tokens[0].Substring(0, "LINE".Length));
                }
                else
                    myset_type = StrToElem(elem_tokens[0]);

                //parse out [optional] color attribute
                string[] elem_tokens2 = elem_tokens[1].Split(new char[] { '#' });

                //got something, but not a valid color format, so throw
                if ((elem_tokens2.Length != MIN_TOKEN_COUNT) && (elem_tokens2.Length != 1))
                    throw new GraphElementFormatException("bad LINE element format: expected a valid color?");

                //no color was specified, so set value
                if (elem_tokens2.Length == 1)
                    val = elem_tokens[1].Trim();
                else
                {   //otherwise set the value and color
                    val = elem_tokens2[0];
                    color = System.Drawing.ColorTranslator.FromHtml('#' + elem_tokens2[1]);
                }

                if(elem_tokens.Length > MIN_TOKEN_COUNT)
                  token_closure(elem_tokens);
            }
            catch (Exception ex)
            {
                if (ex is GraphElementFormatException)
                    throw;
                else
                    throw new GraphElementFormatException("Bad graph element format: ", ex);
            }
        }


      
        public override IGRAPH_ELEMENT DeepClone()
        {
           LINE temp = new  LINE(this.width, ((new StringBuilder()).Append(this.val)).ToString(), this.color,
                                ((new StringBuilder()).Append(this.legend)).ToString(), this.stack);
           temp.dashes = this.dashes;
           temp.dash_offset = this.dash_offset;
           temp.myset_type = this.ELMtype;

           return temp;
        }

         public static LINE DeepClone(LINE elem)
         {
            LINE temp = new  LINE(elem.width, ((new StringBuilder()).Append(elem.val)).ToString(), elem.color,
                                 ((new StringBuilder()).Append(elem.legend)).ToString(), elem.stack);
            temp.dashes      = elem.dashes;
            temp.dash_offset = elem.dash_offset;
            temp.myset_type  = elem.ELMtype;

            return temp;   
         } 


        public override string ToString()
        {
            string STACK        = (stack) ? ":STACK" : "";
            string COLOR        = (color.IsEmpty) ? "" : ColorToHex(color);
            string DASHES;

            if (dashes == "") 
                DASHES = "";
            else if(dashes == "dashes")
                DASHES = ":" + dashes;
            else
                DASHES = ":dashes=" + dashes;

            string WIDTH        = (width > 0.0) ? width.ToString() : "";
            string DASH_OFFSET  = (dash_offset > 0) ? ":dash-offset=" + dash_offset.ToString() : "";

            if (legend == "")
                return ElemToStr(ELMtype) + WIDTH + ":" + this.val + COLOR + STACK + DASHES + DASH_OFFSET;
            else
                return ElemToStr(ELMtype) + WIDTH + ":" + this.val + COLOR + ":" + legend + STACK + DASHES + DASH_OFFSET;
        }


        public double width
        {
            get { return width_; }
            set
            {
                if (value < 0.0)
                    throw new GraphElementFormatException("LINE element width property cannot be negative");

                width_ = value;
            }
        }

#if TEST_LINE_ELEMENT
        public static void Main()
        {
            Console.WriteLine("line1: Testing promotion ctor # 1");
            LINE line1 = new LINE("line_def1", null);
            Console.WriteLine("Hashcode         :      {0}", line1.GetHashCode());
            Console.WriteLine("Value            :      {0}", line1.val);
            Console.WriteLine("Line width       :      {0}", line1.width);
            Console.WriteLine("Color            :      {0}", line1.color.ToString());
            Console.WriteLine("Type             :      {0}", line1.ELMtype);
            Console.WriteLine("Legend           :      {0}", line1.legend);
            Console.WriteLine("STACK set ?      :      {0}", line1.stack);
            Console.WriteLine("Dashes           :      {0}", line1.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line1.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line1.ToString());
         
            Console.WriteLine("\nTesting exception when setting a legend having not specified a color");
            try
            {
                line1.legend = "hello mike";
            }
            catch (GraphElementFormatException ex)
            {
                Console.WriteLine("In handler: {0}", ex.Message);
            }

            Console.WriteLine("\nTesting bad width option exception");
            try
            {
                line1.width = -2;
            }
            catch (GraphElementFormatException ex)
            {
                Console.WriteLine("In handler: {0}", ex.Message);
            }
            

            Console.WriteLine("\nTest Deserialization for \"line1\"");
            LINE line2 = new LINE(line1.ToString());
            Console.WriteLine("Hashcode         :      {0}", line2.GetHashCode());
            Console.WriteLine("Value            :      {0}", line2.val);
            Console.WriteLine("Line width       :      {0}", line2.width);
            Console.WriteLine("Color            :      {0}", line2.color.ToString());
            Console.WriteLine("Type             :      {0}", line2.ELMtype);
            Console.WriteLine("Legend           :      {0}", line2.legend);
            Console.WriteLine("STACK set ?      :      {0}", line2.stack);
            Console.WriteLine("Dashes           :      {0}", line2.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line2.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line2.ToString());


            Console.WriteLine("\n\nline3: Testing promotion ctor # 2");
            LINE line3 = new LINE("line_def3", Color.Gainsboro);
            Console.WriteLine("Hashcode         :      {0}", line3.GetHashCode());
            Console.WriteLine("Value            :      {0}", line3.val);
            Console.WriteLine("Line width       :      {0}", line3.width);
            Console.WriteLine("Color            :      {0}", line3.color.ToString());
            Console.WriteLine("Type             :      {0}", line3.ELMtype);
            Console.WriteLine("Legend           :      {0}", line3.legend);
            Console.WriteLine("STACK set ?      :      {0}", line3.stack);
            Console.WriteLine("Dashes           :      {0}", line3.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line3.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line3.ToString());

            Console.WriteLine("\nTest Deserialization for \"line3\"");
            LINE line4 = new LINE(line3.ToString());
            Console.WriteLine("Hashcode         :      {0}", line4.GetHashCode());
            Console.WriteLine("Value            :      {0}", line4.val);
            Console.WriteLine("Line width       :      {0}", line4.width);
            Console.WriteLine("Color            :      {0}", line4.color.ToString());
            Console.WriteLine("Type             :      {0}", line4.ELMtype);
            Console.WriteLine("Legend           :      {0}", line4.legend);
            Console.WriteLine("STACK set ?      :      {0}", line4.stack);
            Console.WriteLine("Dashes           :      {0}", line4.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line4.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line4.ToString());

            Console.WriteLine("\n\nline5: Testing promotion ctor # 3");
            LINE line5 = new LINE(1, "line_def5", Color.Gainsboro);
            Console.WriteLine("Hashcode         :      {0}", line5.GetHashCode());
            Console.WriteLine("Value            :      {0}", line5.val);
            Console.WriteLine("Line width       :      {0}", line5.width);
            Console.WriteLine("Color            :      {0}", line5.color.ToString());
            Console.WriteLine("Type             :      {0}", line5.ELMtype);
            Console.WriteLine("Legend           :      {0}", line5.legend);
            Console.WriteLine("STACK set ?      :      {0}", line5.stack);
            Console.WriteLine("Dashes           :      {0}", line5.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line5.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line5.ToString());

            Console.WriteLine("\nTest Deserialization for \"line5\"");
            LINE line6 = new LINE(line5.ToString());
            Console.WriteLine("Hashcode         :      {0}", line6.GetHashCode());
            Console.WriteLine("Value            :      {0}", line6.val);
            Console.WriteLine("Line width       :      {0}", line6.width);
            Console.WriteLine("Color            :      {0}", line6.color.ToString());
            Console.WriteLine("Type             :      {0}", line6.ELMtype);
            Console.WriteLine("Legend           :      {0}", line6.legend);
            Console.WriteLine("STACK set ?      :      {0}", line6.stack);
            Console.WriteLine("Dashes           :      {0}", line6.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line6.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line6.ToString());


            Console.WriteLine("\n\nline7: Testing promotion ctor # 4");
            LINE line7 = new LINE(1, "line_def7", Color.Gainsboro, "this is a test", false);
            Console.WriteLine("Hashcode         :      {0}", line7.GetHashCode());
            Console.WriteLine("Value            :      {0}", line7.val);
            Console.WriteLine("Line width       :      {0}", line7.width);
            Console.WriteLine("Color            :      {0}", line7.color.ToString());
            Console.WriteLine("Type             :      {0}", line7.ELMtype);
            Console.WriteLine("Legend           :      {0}", line7.legend);
            Console.WriteLine("STACK set ?      :      {0}", line7.stack);
            Console.WriteLine("Dashes           :      {0}", line7.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line7.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line7.ToString());

            Console.WriteLine("\nTest Deserialization for \"line7\"");
            LINE line8 = new LINE(line7.ToString());
            Console.WriteLine("Hashcode         :      {0}", line8.GetHashCode());
            Console.WriteLine("Value            :      {0}", line8.val);
            Console.WriteLine("Line width       :      {0}", line8.width);
            Console.WriteLine("Color            :      {0}", line8.color.ToString());
            Console.WriteLine("Type             :      {0}", line8.ELMtype);
            Console.WriteLine("Legend           :      {0}", line8.legend);
            Console.WriteLine("STACK set ?      :      {0}", line8.stack);
            Console.WriteLine("Dashes           :      {0}", line8.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line8.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line8.ToString());

            Console.WriteLine("\nTesting mutators on: \"line8\" ");
            line8.stack = false;
            line8.set_dashes(true);
            line8.dash_offset = 10;
            Console.WriteLine("Hashcode         :      {0}", line8.GetHashCode());
            Console.WriteLine("Value            :      {0}", line8.val);
            Console.WriteLine("Line width       :      {0}", line8.width);
            Console.WriteLine("Color            :      {0}", line8.color.ToString());
            Console.WriteLine("Type             :      {0}", line8.ELMtype);
            Console.WriteLine("Legend           :      {0}", line8.legend);
            Console.WriteLine("STACK set ?      :      {0}", line8.stack);
            Console.WriteLine("Dashes           :      {0}", line8.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line8.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line8.ToString());


            Console.WriteLine("\nTest Deserialization for \"line8\"");
            LINE line9 = new LINE(line8.ToString());
            Console.WriteLine("Hashcode         :      {0}", line9.GetHashCode());
            Console.WriteLine("Value            :      {0}", line9.val);
            Console.WriteLine("Line width       :      {0}", line9.width);
            Console.WriteLine("Color            :      {0}", line9.color.ToString());
            Console.WriteLine("Type             :      {0}", line9.ELMtype);
            Console.WriteLine("Legend           :      {0}", line9.legend);
            Console.WriteLine("STACK set ?      :      {0}", line9.stack);
            Console.WriteLine("Dashes           :      {0}", line9.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line9.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line9.ToString());


            Console.WriteLine("\n\nline10: Testing promotion ctor # 5");
            LINE line10 = new LINE(2, "line_def10", Color.Gainsboro, ":this:is: a: test:", true,true);
            Console.WriteLine("Hashcode         :      {0}", line10.GetHashCode());
            Console.WriteLine("Value            :      {0}", line10.val);
            Console.WriteLine("Line width       :      {0}", line10.width);
            Console.WriteLine("Color            :      {0}", line10.color.ToString());
            Console.WriteLine("Type             :      {0}", line10.ELMtype);
            Console.WriteLine("Legend           :      {0}", line10.legend);
            Console.WriteLine("STACK set ?      :      {0}", line10.stack);
            Console.WriteLine("Dashes           :      {0}", line10.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line10.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line10.ToString());


            Console.WriteLine("\nTest Deserialization for \"line10\"");
            LINE line11 = new LINE(line10.ToString());
            Console.WriteLine("Hashcode         :      {0}", line11.GetHashCode());
            Console.WriteLine("Value            :      {0}", line11.val);
            Console.WriteLine("Line width       :      {0}", line11.width);
            Console.WriteLine("Color            :      {0}", line11.color.ToString());
            Console.WriteLine("Type             :      {0}", line11.ELMtype);
            Console.WriteLine("Legend           :      {0}", line11.legend);
            Console.WriteLine("STACK set ?      :      {0}", line11.stack);
            Console.WriteLine("Dashes           :      {0}", line11.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line11.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line11.ToString());



            Console.WriteLine("\n\nline12: Testing promotion ctor # 6");
            LINE line12 = new LINE(2, "line_def12", Color.Gainsboro, ":this:is: a: test:", true, (new int[] {10,5,5,5}));
            Console.WriteLine("Hashcode         :      {0}", line12.GetHashCode());
            Console.WriteLine("Value            :      {0}", line12.val);
            Console.WriteLine("Line width       :      {0}", line12.width);
            Console.WriteLine("Color            :      {0}", line12.color.ToString());
            Console.WriteLine("Type             :      {0}", line12.ELMtype);
            Console.WriteLine("Legend           :      {0}", line12.legend);
            Console.WriteLine("STACK set ?      :      {0}", line12.stack);
            Console.WriteLine("Dashes           :      {0}", line12.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line12.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line12.ToString());


            Console.WriteLine("\nTest Deserialization for \"line12\"");
            LINE line13 = new LINE(line12.ToString());
            Console.WriteLine("Hashcode         :      {0}", line13.GetHashCode());
            Console.WriteLine("Value            :      {0}", line13.val);
            Console.WriteLine("Line width       :      {0}", line13.width);
            Console.WriteLine("Color            :      {0}", line13.color.ToString());
            Console.WriteLine("Type             :      {0}", line13.ELMtype);
            Console.WriteLine("Legend           :      {0}", line13.legend);
            Console.WriteLine("STACK set ?      :      {0}", line13.stack);
            Console.WriteLine("Dashes           :      {0}", line13.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line13.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line13.ToString());


            Console.WriteLine("\n\nline14: Testing promotion ctor # 7");
            LINE line14 = new LINE(2.5, "line_def15", Color.AliceBlue, "test_label_for_14", true, (new int[] { 10, 5, 5, 5 }), 10);
            Console.WriteLine("Hashcode         :      {0}", line14.GetHashCode());
            Console.WriteLine("Value            :      {0}", line14.val);
            Console.WriteLine("Line width       :      {0}", line14.width);
            Console.WriteLine("Color            :      {0}", line14.color.ToString());
            Console.WriteLine("Type             :      {0}", line14.ELMtype);
            Console.WriteLine("Legend           :      {0}", line14.legend);
            Console.WriteLine("STACK set ?      :      {0}", line14.stack);
            Console.WriteLine("Dashes           :      {0}", line14.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line14.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line14.ToString());


            Console.WriteLine("\nTest Deserialization for \"line14\"");
            LINE line15 = new LINE(line14.ToString());
            Console.WriteLine("Hashcode         :      {0}", line15.GetHashCode());
            Console.WriteLine("Value            :      {0}", line15.val);
            Console.WriteLine("Line width       :      {0}", line15.width);
            Console.WriteLine("Color            :      {0}", line15.color.ToString());
            Console.WriteLine("Type             :      {0}", line15.ELMtype);
            Console.WriteLine("Legend           :      {0}", line15.legend);
            Console.WriteLine("STACK set ?      :      {0}", line15.stack);
            Console.WriteLine("Dashes           :      {0}", line15.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line15.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line15.ToString());



            Console.WriteLine("\nTesting DeepClone and Deserialization ");
            LINE line16 = new LINE(line15.DeepClone().ToString());
            Console.WriteLine("Hashcode         :      {0}", line16.GetHashCode());
            Console.WriteLine("Value            :      {0}", line16.val);
            Console.WriteLine("Line width       :      {0}", line16.width);
            Console.WriteLine("Color            :      {0}", line16.color.ToString());
            Console.WriteLine("Type             :      {0}", line16.ELMtype);
            Console.WriteLine("Legend           :      {0}", line16.legend);
            Console.WriteLine("STACK set ?      :      {0}", line16.stack);
            Console.WriteLine("Dashes           :      {0}", line16.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line16.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line16.ToString());

           
            Console.WriteLine("\nTesting class mutators");
            line16.stack = false;
            line16.set_dashes(true);
            line16.dash_offset = -1;
            Console.WriteLine("Hashcode         :      {0}", line16.GetHashCode());
            Console.WriteLine("Value            :      {0}", line16.val);
            Console.WriteLine("Line width       :      {0}", line16.width);
            Console.WriteLine("Color            :      {0}", line16.color.ToString());
            Console.WriteLine("Type             :      {0}", line16.ELMtype);
            Console.WriteLine("Legend           :      {0}", line16.legend);
            Console.WriteLine("STACK set ?      :      {0}", line16.stack);
            Console.WriteLine("Dashes           :      {0}", line16.dashes);
            Console.WriteLine("Dash Offset      :      {0}", line16.dash_offset);
            Console.WriteLine("Serialization    :      {0}", line16.ToString());

        }

            
#endif
    }
}
