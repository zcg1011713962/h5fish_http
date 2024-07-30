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
//File       : AREA.cs   -- .Net / Mono AREA class                                                            //
//Application: NHawk: Open Source Project Support                                                             // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                                //
//Author     : Mike Corley, Syracuse University                                                               //
//             mwcorley@syr.edu                                                                               //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides the class definition for the NHAWK AREA construct which constitutes a thin wrapper
 * around the rrdtool AREA graph element construct. It is intended that the public interface intuitively resemble 
 * native rrdtool construct syntax. For usage detail see this interface and rrdtool man page documentation at: 
 * http://oss.oetiker.ch/rrdtool/doc/rrdgraph_graph.en.html.
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
 * public class AREA : AGRAPH_ELEMENT  <-- derived class AREA
 * 
 * public AREA(string val, object set_to_null)
 * -- constructor:  accepts val (the basic def). e.g AREA:[val][variable],
 *                  val        : can be a static value or valid variable as defined by a DEF, CDEF, or VDEF
 *                  set_to_null: should always be set to null (this is just place holder and has no 
 *                               significance other than to provide a unique function prototype\definition,
 *                               which otherwise conflicts with deserializaion ctor signature: "public AREA(string elem)" )
 *  
 * public AREA(string val, Color c)
 * -- constructor: accepts val and color e.g AREA:[1000][variable]#color
 *    val        : can be a static value or valid variable as defined by a DEF, CDEF, VDEF 
 *    color      : the color of visualized area
 * 
 * public AREA(string val, Color c, string legend)
 *  -- constructor: accepts val, color, and legend   e.g AREA:1000#color:"this a a legend"
 *     val        : can be a static value or valid variable as defined by a DEF or CDEF 
 *     color      : the color of the visualized area
 *     legend     : the text label associated the element 
 *                  note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                        a GraphFormatException will be thrown
 *                      
 * public AREA(string val, Color c, string legend, bool stack)
 *  -- constructor: accepts val_name, color, legend, and stack     e.g AREA:1000#color:"this a a legend":STACK
 *     val        : can be a static value or valid variable as defined by a DEF, CDEF, or VDEF 
 *     color      : the color of the visualized area
 *     legend     : the text label associated the element 
 *                  note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                        a GraphElementFormatException will be thrown
 *     stack      : a boolean which determines whether or not this element is stacked above
 *                  the previously defined graph element
 * 
 * public AREA(string element)
 *   -- deserialization constructor:  accepts a raw rrdtool level AREA element construct as a string and deserializes it resulting
 *                                    in a fully constructed AREA object.  The object can then be manipulated and serialized once 
 *                                    again by calling the ToString() override.  The is used primary by the GRAPH class which will
 *                                    ask each graph element (contained within its definition) to deserialize itself.  Failed attempts
 *                                    to deserialize will result in GraphElementFormatException being thorwn
 *   
 * public override string ToString()
 *   -- serialization override:  serializes the AREA element to a string.  The is called polymorphically by the GRAPH class 
 *                               which will ask each graph element (contained within its definition) to serialize itself so the entire
 *                               GRAPH structure (as a composite structure), can be serialized and sent to rrdtool.exe for processing
 *                               
 * public override GRAPH_ELEMENT DeepClone()
 *   -- deep clone override: polymorphically returns a (new memory) copy of the AREA element. This is used by the GRAPH class for adding
 *                           elements to the element list for tracking.  
 *                           Note on DeepCloning: In the NHAWK model, the DeepClone works in the shallow reference model because every member
 *                           member stored is either value type or string reference type and new string objects are construced, initialized and 
 *                           returned with temporary StringBuilder(), with it's ToString() method is called to yield the new string instance.
 *                           This is useful because graph definitions won't be susceptible to potential modification by external defined references.
 *                           
 * 
 * public static AREA DeepClone(AREA elem)
 *    -- provides a the same operation as "GRAPH_ELEMENT DeepClone()", but doesn't work polymorphically.  
 *       This is generally the preferred version that would be called directly by clients.
 *       
 * public void set_dashes(int[] dashes_args)
 *    -- not defined for AREA, calling it will have no effect
 *                 
 *   public int dash_offset
 *    -- not defined for AREA, calling will have no effect
 *      
 *   public TYPE type
 *   -- gets or the element type: AREA
 *    
 *   public Color color
 *   -- gets or sets the color of the visualizable element
 *      e.g. AREA:1000#color:"this a a legend"
 *    
 *   public string val
 *   -- gets or sets the graph definition variable or value.  
 *      val must consist of a contiguous sequence of characters (no spaces) 
 *      or GraphElementFormatException will be thrown.
 *      e.g. AREA:val
 *   
 *   public bool stack
 *   -- gets or sets a boolean which determines whether or not this element is stacked above
 *      the previously defined graph element
 *      AREA:1000#color:"this a a legend"
 *         or
 *      AREA:1000#color:"this a a legend":STACK
 *      
 *   public string legend
 *      --gets or sets the legend. Note:  if specified when color == Color.IsEmpty == true, and
 *        a GraphFormatException will be thrown.
 *     AREA:1000#color:"this a a legend"  
 * 
 * Build Process
 * =============
 * This module can be compiled stand alone for testing purposes: 
 * Note: some modules have additional assemblies dependencies which
 * are needed for compilation of the test stub.  The required files section
 * shows true dependency structure. i.e when the test stub is not compiled.
 *
 *
 * Compiler Command:
 * Visual Studio 2008 (.Net 3.5): csc /define:TEST_AREA_ELEMENT /reference:c:\windows\microsoft.net\framework\v2.0.50727\Microsoft.VisualBasic.dll 
 *                                   ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs AREA.cs  
 *    
 * Win32 Mono (1.9.1)           : gmcs -define:TEST_LINE_ELEMENT -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.Drawing.dll 
 *                                                               -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\Microsoft.VisualBasic.dll
 *                                                               -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.dll 
 *                                                               ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs AREA.cs
 *                                                                   
 * SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_LINE_ELEMENT -r:/usr/lib/mono/gac/System/2.0.0.0__b77a5c561934e089/System.dll 
 *                                                               -r:/usr/lib/mono/gac/System.Drawing/2.0.0.0__b03f5f7f11d50a3a/System.Drawing.dll 
 *                                                               -r:/usr/lib/mono/gac/Microsoft.VisualBasic/8.0.0.0__b03f5f7f11d50a3a/Microsoft.VisualBasic.dll  
 *                                                               ../GRAPH_ELEMENT/GRAPH_ELEMENT.cs AREA.cs                                              
 *                                                                  
 * Required Files:   AREA.cs GRAPH_ELEMENT.cs
 * 
 * Maintanence History
 * =================== 
 * version 1.0 : 01 Aug 08 
 *    -- first release: 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NHAWK
{
    [Serializable]
    public class AREA : AGRAPH_ELEMENT
    {
        public AREA(string val, object set_to_null)
            : base(AGRAPH_ELEMENT.TYPE.AREA, val)
        {}

        public AREA(string val, Color c)
            : base(AGRAPH_ELEMENT.TYPE.AREA, val, c)
        {}

        public AREA(string val, Color c, string legend)
            : base(AGRAPH_ELEMENT.TYPE.AREA, val, c, legend,false)
        {}

        public AREA(string val, Color c, string legend, bool stack)
            : base(AGRAPH_ELEMENT.TYPE.AREA, val, c, legend, stack)
        {}


        public AREA(string element): base()
        {
            try
            {
                //parse out all tokens (separated by ':' )
                string[] elem_tokens = element.Split(new char[] { ':' });
   
                //check that the minumum number tokens exist
                if (elem_tokens.Length < MIN_TOKEN_COUNT)
                    throw new GraphElementFormatException("bad AREA element format");
   
                //get the type of the element (should be AREA)
                myset_type = StrToElem(elem_tokens[0]);
                if (ELMtype != TYPE.AREA)
                    throw new GraphElementFormatException("Element type should be AREA?");

                //parse the optional coloir field
                string[] elem_tokens2 = elem_tokens[1].Split(new char[] { '#' });
               
                //if color was not specified, then set the AREA value field and return
                if ((elem_tokens2.Length != MIN_TOKEN_COUNT) && (elem_tokens2.Length != 1))
                    throw new GraphElementFormatException("bad AREA element format: expected a valid color?");

                if (elem_tokens2.Length == 1)
                {
                    val = elem_tokens[1].Trim();
                    return;
                }
                //a color was specified, so continue processing
                this.val = elem_tokens2[0];
                color = System.Drawing.ColorTranslator.FromHtml('#' + elem_tokens2[1]);
                
                //determine of a multiword legend was specified and/or if STACK attribute was specified
                //perform forward token closure on remainder of the token sequenbce
                if (elem_tokens.Length > MIN_TOKEN_COUNT)
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

      
        public override string ToString()
        {
            string STACK = (stack)         ? ":STACK" : "";
            string COLOR = (color.IsEmpty) ? "" : ColorToHex(color);

            if (legend == "")
                return ElemToStr(ELMtype) + ":" + val + COLOR + STACK;
            else
                return ElemToStr(ELMtype) + ":" + val + COLOR + ":" + legend + STACK;
        }


        public override IGRAPH_ELEMENT DeepClone()
        {
            return new AREA(((new StringBuilder()).Append(this.val)).ToString(), this.color,
                             ((new StringBuilder()).Append(this.legend)).ToString(), this.stack);
        }


        public static AREA DeepClone(AREA elem)
        {
            return new AREA( ((new StringBuilder()).Append(elem.val)).ToString(), elem.color, 
                             ((new StringBuilder()).Append(elem.legend)).ToString(), elem.stack);
        }

      

#if TEST_AREA_ELEMENT
        public static void Main()
        {
            Console.WriteLine("Testing promotion ctor # 1 (no legends and colors)");
            AREA area1 = new AREA("elem_name1", null);
           
            Console.WriteLine("Hashcode         :      {0}", area1.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", area1.val);
            Console.WriteLine("Color            :      {0}", area1.color.ToString());
            Console.WriteLine("Type             :      {0}", area1.ELMtype);
            Console.WriteLine("Legend           :      {0}", area1.legend);
            Console.WriteLine("STACK set ?      :      {0}", area1.stack);
            Console.WriteLine("Serialization    :      {0}", area1.ToString());

            Console.WriteLine("\nTesting exception when setting a legend having not specified a color");
            try
            {
                area1.legend = "hello mike";
            }
            catch (GraphElementFormatException ex)
            {
                Console.WriteLine("In handler: {0}", ex.Message);
            }


            Console.WriteLine("\nTesting DeepClone for area1");
            AREA elem_copy = AREA.DeepClone(area1);
            Console.WriteLine("Hashcode         :      {0}", elem_copy.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem_copy.val);
            Console.WriteLine("Color            :      {0}", elem_copy.color.ToString());
            Console.WriteLine("Type             :      {0}", elem_copy.ELMtype);
            Console.WriteLine("Legend           :      {0}", elem_copy.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem_copy.stack);
            Console.WriteLine("Serialization    :      {0}", elem_copy.ToString());

            Console.WriteLine("\nTesting Deserialization");
            AREA elem_copy2 = new AREA(elem_copy.ToString());
            Console.WriteLine("Hashcode         :      {0}", elem_copy2.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem_copy2.val);
            Console.WriteLine("Color            :      {0}", elem_copy2.color.ToString());
            Console.WriteLine("Type             :      {0}", elem_copy2.ELMtype);
            Console.WriteLine("Legend           :      {0}", elem_copy2.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem_copy2.stack);
            Console.WriteLine("Serialization    :      {0}", elem_copy2.ToString());

            Console.WriteLine("\nTesting class mutators");
            elem_copy2.val = "some_def";
            elem_copy2.color = Color.Aqua;
            //elem_copy2._type = TYPE.AREA;

            Console.WriteLine("Hashcode         :      {0}", elem_copy2.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem_copy2.val);
            Console.WriteLine("Color            :      {0}", elem_copy2.color.ToString());
            Console.WriteLine("Type             :      {0}", elem_copy2.ELMtype);
            Console.WriteLine("Legend           :      {0}", elem_copy2.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem_copy2.stack);
            Console.WriteLine("Serialization    :      {0}", elem_copy2.ToString());


            Console.WriteLine("\nelem2: Testing promotion ctor # 2");
            AREA elem2 = new AREA("def2", Color.Firebrick);
            Console.WriteLine("Hashcode         :      {0}", elem2.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem2.val);
            Console.WriteLine("Color            :      {0}", elem2.color.ToString());
            Console.WriteLine("Type             :      {0}", elem2.ELMtype);
            Console.WriteLine("legend           :      {0}", elem2.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem2.stack);
            Console.WriteLine("Serialization    :      {0}", elem2.ToString());


            Console.WriteLine("\nTesting Deserialization of elem2");
            AREA elem3 = new AREA(elem2.ToString());
           // elem3.legend = "test legend: for def2, but elem3";
            Console.WriteLine("Hashcode         :      {0}", elem3.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem3.val);
            Console.WriteLine("Color            :      {0}", elem3.color.ToString());
            Console.WriteLine("Type             :      {0}", elem3.ELMtype);
            Console.WriteLine("legend           :      {0}", elem3.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem2.stack);
            Console.WriteLine("Serialization    :      {0}", elem3.ToString());

            Console.WriteLine("\nelem4: Testing promotion ctor # 3");
            AREA elem4 = new AREA("def4", Color.Black, "test: legend: for def2");
            Console.WriteLine("Hashcode         :      {0}", elem4.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem4.val);
            Console.WriteLine("Color            :      {0}", elem4.color.ToString());
            Console.WriteLine("Type             :      {0}", elem4.ELMtype);
            Console.WriteLine("legend           :      {0}", elem4.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem4.stack);
            Console.WriteLine("Serialization    :      {0}", elem4.ToString());

            Console.WriteLine("\nTesting Deserialization of elem4");
            AREA elem5 = new AREA(elem4.ToString());
            //elem5.legend = "";  // ** <-- notice this cause stack attrib to set false **
            Console.WriteLine("Hashcode         :      {0}", elem5.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem5.val);
            Console.WriteLine("Color            :      {0}", elem5.color.ToString());
            Console.WriteLine("Type             :      {0}", elem5.ELMtype);
            Console.WriteLine("legend           :      {0}", elem5.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem5.stack);
            Console.WriteLine("Serialization    :      {0}", elem5.ToString());

            Console.WriteLine("\nelem6: Testing promotion ctor # 4");
            AREA elem6 = new AREA("def6", Color.Black, "test: legend: for def2", true);
            Console.WriteLine("Hashcode         :      {0}", elem6.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem6.val);
            Console.WriteLine("Color            :      {0}", elem6.color.ToString());
            Console.WriteLine("Type             :      {0}", elem6.ELMtype);
            Console.WriteLine("legend           :      {0}", elem6.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem6.stack);
            Console.WriteLine("Serialization    :      {0}", elem6.ToString());

            Console.WriteLine("\nTesting Deserialization of elem6");
            AREA elem7 = new AREA(elem6.ToString());
            //elem7.legend = "";  // ** <-- notice this causes stack attrib to set false **

            Console.WriteLine("Hashcode         :      {0}", elem7.GetHashCode());
            Console.WriteLine("DEF name         :      {0}", elem7.val);
            Console.WriteLine("Color            :      {0}", elem7.color.ToString());
            Console.WriteLine("Type             :      {0}", elem7.ELMtype);
            Console.WriteLine("legend           :      {0}", elem7.legend);
            Console.WriteLine("STACK set ?      :      {0}", elem7.stack);
            Console.WriteLine("Serialization    :      {0}", elem7.ToString());

            

        }

            
#endif 
    }
}
