﻿/* 
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

/////////////////////////////////////////////////////////////////////////////////////////////////
//File       : VRULE.cs   -- provides .Net / Mono wrapper around the RRDTool VRULE Construct   //
//Application: NHawk: Open Source Project Support                                              // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                 //
//Author     : Mike Corley, Syracuse University                                                //
//             mwcorley@syr.edu                                                                //
/////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides the class definition for the NHAWK VRULE construct, which constitutes a thin wrapper
 * around the rrdtool VRULE construct. It is intended that the public interface intuitively resemble native rrdtool 
 * construct syntax. For usage detail see this interface and rrdtool man page documentation at: 
 * http://oss.oetiker.ch/rrdtool/doc/rrdgraph_data.en.html
 * 
 * Note about this class and the NHAWK project: This class seeks to encapsulate rrdtool constructs the in C# .Net / Mono 
 * object model while retaining intuitive use of the RRDTool syntax. It also seeks to enforce correct usage of rrdtool
 * constructs by providing semantically appropriate exception handling.  The primary goal of the NHAWK project is to enable
 * .Net developers to use RRDTool directly from C#, while retaining the native (intuitive use) of rrdtool construct sytnax.  
 * It is our intent that NHAWK constitute a very thin rrdtool provider layer, enabling proficient .Net developers to rapidly 
 * gain productivity by referring directly (and intuitively) to the main rrdtool documentation page at: 
 * http://oss.oetiker.ch/rrdtool/doc/rrdgraph.en.html
 * 
 * Public Interface
 * ================
 *  
 *  [Serializable]
 *  public class VRULE : AGRAPH_ELEMENT
 * 
 *  public VRULE(string time_val, Color c)
 *  -- constructor: accepts time_val, and color  e.g VRULE:time#color
 *     time_val   : can be a time value or valid variable as defined by a VDEF but not a CDEF or DEF 
 *     color      : the color of line
 *     
 * public VRULE(string time_val, Color c, string legend)
 *  -- constructor: accepts time_val, color, and legend   e.g VRULE:time#color:"this a a legend"
 *     time_val   : can be a time value or valid variable as defined by a VDEF but not a CDEF or DEF 
 *     color      : the color of the visualized area
 *     legend     : the text label associated the element 
 *                  note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                        a GraphFormatException will be thrown
 * 
 * public VRULE(string time_val, Color c, string legend)
 * -- constructor: time_val, color, legend     e.g VRULE:time#color:"this a a legend" 
 *    time_val   : can be a time value or valid variable as defined by a VDEF but not a CDEF or DEF 
 *    color      : the color of the visualized area
 *    legend     : the text label associated the element 
 *                  note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                        a GraphFormatException will be thrown
 *                
 * public VRULE(string time_val, Color c, string legend,  bool dashes_val)
 * -- constructor: accepts time_val, color, legend, stack, dashes_val e.g VRULE:time#color:"this a a legend":STACK:dashes
 *    time_val   : can be a time value or valid variable as defined by a VDEF but not a CDEF or DEF 
 *    color      : the color of the visualized area
 *    legend     : the text label associated the element 
 *                 note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                       a GraphFormatException will be thrown
 *    
 *    dashes_val : if set to true, the dashes option will be set causing the LINE element to be displayed with dashes
 * 
 * public VRULE(string time_val, Color c, string legend, int[] dashes_val)
 * -- constructor: accepts time_val, color, legend, dashes_val e.g  VRULE:time#color:"this a a legend":STACK:dashes=10,5,5,10
 *    time_val   : can be a time value or valid variable as defined by a VDEF but not a CDEF or DEF 
 *    color      : the color of the visualized area
 *    legend     : the text label associated the element 
 *                 note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                       a GraphFormatException will be thrown
 *    dashes_val : array of integer parameters, that affact appearance and dash thickness (see rrdtool docs)
 *    
 * public VRULE(string time_val, Color c, string legend, int[] dashes_val, int dashes_offset)
 * -- constructor: accepts time_val, color, legend, stack, dashes_val e.g VRULE:time#color:"this a a legend":dashes=10,5,5,10:20
 *    time_val   : can be a time value or valid variable as defined by a VDEF but not a CDEF or DEF 
 *    color      : the color of the visualized area
 *    legend     : the text label associated the element 
 *                 note: if legend is specified without defining a color (color == (Color.IsEmpty == true)), 
 *                       a GraphFormatException will be thrown
 *    dashes_val   : array of integer parameters, that affact appearance and dash thickness of the line (see rrdtool docs)
 *    dashes_offset: determines the value at which the dashed line begins
 *    
 *  public string time_val
 *  -- gets or sets the time_val
 *  
 * 
 *  Build Process
 *  =============
 *  This module can be compiled stand alone for testing purposes: 
 *  Note: some modules have additional assemblies dependencies which
 *       are needed for compilation of the test stub.  The required files section
 *       shows true dependency structure. i.e when the test stub is not compiled.
 *
 *  Compiler Command:
 *  Visual Studio 2008 (.Net 3.5): csc /define:TEST_VRULE /reference:c:\windows\microsoft.net\framework\v2.0.50727\Microsoft.VisualBasic.dll 
 *                                                      ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs VRULE.cs  
 *    
 *  Win32 Mono (1.9.1)           : gmcs -define:TEST_VRULE -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.Drawing.dll 
 *                                                         -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\Microsoft.VisualBasic.dll
 *                                                         -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.dll 
 *                                                         ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs VRULE.cs
 *                                                                   
 *  SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_VRULE -r:/usr/lib/mono/gac/System/2.0.0.0__b77a5c561934e089/System.dll 
 *                                                         -r:/usr/lib/mono/gac/System.Drawing/2.0.0.0__b03f5f7f11d50a3a/System.Drawing.dll 
 *                                                         -r:/usr/lib/mono/gac/Microsoft.VisualBasic/8.0.0.0__b03f5f7f11d50a3a/Microsoft.VisualBasic.dll  
 *                                                         ../GRAPH_ELEMENT/GRAPH_ELEMENT.cs VRULE.cs                                              
 *                                                                  
 *  Required Files:   VRULE.cs GRAPH_ELEMENT.cs
 * 
 *  Example Usage
 *  =============
 *  See Test stub below
 * 
 * 
 *  Maintanence History
 *  ===================
 *  version 1.0 : 01 August 08 
 *     -- first release: 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NHAWK
{
    [Serializable]
    public class VRULE : AGRAPH_ELEMENT
    {
      
       private VRULE()
            : base(TYPE.VRULE, "")
        {}
        
        public VRULE(string val, Color c)
            : base(TYPE.VRULE, val, c)
        {}

        public VRULE(string val, Color c, string legend)
            : base(TYPE.VRULE, val, c,legend,false)
        {}

        public VRULE(string val, Color c, string legend, bool dashes)
            : base(TYPE.VRULE, val, c,legend, false, dashes)
        {}
        
        public VRULE(string val, Color c, string legend, int[] dashes)
                 : base(TYPE.VRULE, val, c,legend, false, dashes)
        {}

        public VRULE(string val, Color c, string legend, int[] dashes, int dash_offset)
                 : base(TYPE.VRULE, val, c,legend, false, dashes, dash_offset)
        {}

        public VRULE(string element)
            : this()
        {
            try
            {
                //parse out all tokens (separated by ':' )
                string[] elem_tokens = element.Split(new char[] { ':' });

                //check that the minumum number of tokens exist
                if (elem_tokens.Length < MIN_TOKEN_COUNT)
                    throw new GraphElementFormatException("bad LINE element format");

                //parse out [optional] color attribute
                string[] elem_tokens2 = elem_tokens[1].Split(new char[] { '#' });

                //got something, but not a valid color format, so throw
                if (elem_tokens2.Length != MIN_TOKEN_COUNT)
                    throw new GraphElementFormatException("VRULE element: expected a valid color?");

                val = elem_tokens2[0];
                color = System.Drawing.ColorTranslator.FromHtml('#' + elem_tokens2[1]);


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
        
   
      
        public override IGRAPH_ELEMENT DeepClone()
        {
            VRULE hr = new VRULE(((new StringBuilder()).Append(this.val)).ToString(), this.color,
                             ((new StringBuilder()).Append(this.legend)).ToString(), false);

            hr.myset_type = TYPE.VRULE;

            return hr;
          
        }


        public static VRULE DeepClone(VRULE elem)
        {
            VRULE hr = new VRULE(((new StringBuilder()).Append(elem.val)).ToString(), elem.color,
                            ((new StringBuilder()).Append(elem.legend)).ToString(), false);

            hr.myset_type = TYPE.VRULE;

            return hr;
        }


        public override string ToString()
        {
           
            string COLOR = (color.IsEmpty) ? "" : ColorToHex(color);
            string DASHES;

            if (dashes == "") 
                DASHES = "";
            else if(dashes == "dashes")
                DASHES = ":" + dashes;
            else
                DASHES = ":dashes=" + dashes;

            string DASH_OFFSET  = (dash_offset > 0) ? ":dash-offset=" + dash_offset.ToString() : "";

            if (legend == "")
                return ElemToStr(ELMtype) + ":" + val + COLOR + DASHES + DASH_OFFSET;
            else
                return ElemToStr(ELMtype) + ":" + val + COLOR + ":" + legend + DASHES + DASH_OFFSET;
        }


        public string time_val
        {
            get { return val; }
            set { val = value; }
        }


#if TEST_VRULE
        public static void Main()
        {
            Console.WriteLine("VRULE1: Testing promotion ctor # 1");
            VRULE vrule1 = new VRULE("920804700", Color.AliceBlue);

            Console.WriteLine("Hashcode         :      {0}", vrule1.GetHashCode());
            Console.WriteLine("Time             :      {0}", vrule1.time_val);
            Console.WriteLine("Color            :      {0}", vrule1.color.ToString());
            Console.WriteLine("Type             :      {0}", vrule1.ELMtype);
            Console.WriteLine("Legend           :      {0}", vrule1.legend);
            Console.WriteLine("Dashes           :      {0}", vrule1.dashes);
            Console.WriteLine("Dash Offset      :      {0}", vrule1.dash_offset);
            Console.WriteLine("Serialization    :      {0}", vrule1.ToString());

             
            Console.WriteLine("\nTesting deserialization: for ctor # 1");
            VRULE vrule2 = new VRULE(vrule1.ToString());
            Console.WriteLine("Hashcode         :      {0}", vrule2.GetHashCode());
            Console.WriteLine("Time             :      {0}", vrule2.time_val);
            Console.WriteLine("Color            :      {0}", vrule2.color.ToString());
            Console.WriteLine("Type             :      {0}", vrule2.ELMtype);
            Console.WriteLine("Legend           :      {0}", vrule2.legend);
            Console.WriteLine("Dashes           :      {0}", vrule2.dashes);
            Console.WriteLine("Dash Offset      :      {0}", vrule2.dash_offset);
            Console.WriteLine("Serialization    :      {0}", vrule2.ToString());

            
            Console.WriteLine("\nHRULE3: Testing promotion ctor # 2");
            VRULE vrule3 = new VRULE("920804700", Color.Gainsboro, "test vrule2: legend");

            Console.WriteLine("Hashcode         :      {0}", vrule3.GetHashCode());
            Console.WriteLine("Time             :      {0}", vrule3.time_val);
            Console.WriteLine("Color            :      {0}", vrule3.color.ToString());
            Console.WriteLine("Type             :      {0}", vrule3.ELMtype);
            Console.WriteLine("Legend           :      {0}", vrule3.legend);
            Console.WriteLine("Dashes           :      {0}", vrule3.dashes);
            Console.WriteLine("Dash Offset      :      {0}", vrule3.dash_offset);
            Console.WriteLine("Serialization    :      {0}", vrule3.ToString());

            
            Console.WriteLine("\nTesting deserialization: for ctor # 2");
            VRULE vrule4 = new VRULE(vrule3.ToString());
            Console.WriteLine("Hashcode         :      {0}", vrule4.GetHashCode());
            Console.WriteLine("Time             :      {0}", vrule4.time_val);
            Console.WriteLine("Color            :      {0}", vrule4.color.ToString());
            Console.WriteLine("Type             :      {0}", vrule4.ELMtype);
            Console.WriteLine("Legend           :      {0}", vrule4.legend);
            Console.WriteLine("Dashes           :      {0}", vrule4.dashes);
            Console.WriteLine("Dash Offset      :      {0}", vrule4.dash_offset);
            Console.WriteLine("Serialization    :      {0}", vrule4.ToString());

           
            Console.WriteLine("\nTesting promotion ctor # 3");
            VRULE vrule5 = new VRULE("920804700", Color.Gainsboro, "test vrule3: legend", true);

            Console.WriteLine("Hashcode         :      {0}", vrule5.GetHashCode());
            Console.WriteLine("Time             :      {0}", vrule5.time_val);
            Console.WriteLine("Color            :      {0}", vrule5.color.ToString());
            Console.WriteLine("Type             :      {0}", vrule5.ELMtype);
            Console.WriteLine("Legend           :      {0}", vrule5.legend);
            Console.WriteLine("Dashes           :      {0}", vrule5.dashes);
            Console.WriteLine("Dash Offset      :      {0}", vrule5.dash_offset);
            Console.WriteLine("Serialization    :      {0}", vrule5.ToString());

            
            Console.WriteLine("\nTesting deserialization: for ctor # 3");
            VRULE vrule6 = new VRULE(vrule5.ToString());
            Console.WriteLine("Hashcode         :      {0}", vrule6.GetHashCode());
            Console.WriteLine("Time             :      {0}", vrule6.val);
            Console.WriteLine("Color            :      {0}", vrule6.color.ToString());
            Console.WriteLine("Type             :      {0}", vrule6.ELMtype);
            Console.WriteLine("Legend           :      {0}", vrule6.legend);
            Console.WriteLine("Dashes           :      {0}", vrule6.dashes);
            Console.WriteLine("Dash Offset      :      {0}", vrule6.dash_offset);
            Console.WriteLine("Serialization    :      {0}", vrule6.ToString());

           
            Console.WriteLine("\nTesting promotion ctor # 4");
            VRULE vrule7 = new VRULE("920804700", Color.Gainsboro, "test vrule4: legend", new int[] { 10, 5, 5, 10 });

            Console.WriteLine("Hashcode         :      {0}", vrule7.GetHashCode());
            Console.WriteLine("Time             :      {0}", vrule7.time_val);
            Console.WriteLine("Color            :      {0}", vrule7.color.ToString());
            Console.WriteLine("Type             :      {0}", vrule7.ELMtype);
            Console.WriteLine("Legend           :      {0}", vrule7.legend);
            Console.WriteLine("Dashes           :      {0}", vrule7.dashes);
            Console.WriteLine("Dash Offset      :      {0}", vrule7.dash_offset);
            Console.WriteLine("Serialization    :      {0}", vrule7.ToString());

            Console.WriteLine("\nTesting deserialization: for ctor # 4");
            VRULE vrule8 = new VRULE(vrule7.ToString());
            Console.WriteLine("Hashcode         :      {0}", vrule8.GetHashCode());
            Console.WriteLine("Time             :      {0}", vrule8.val);
            Console.WriteLine("Color            :      {0}", vrule8.color.ToString());
            Console.WriteLine("Type             :      {0}", vrule8.ELMtype);
            Console.WriteLine("Legend           :      {0}", vrule8.legend);
            Console.WriteLine("Dashes           :      {0}", vrule8.dashes);
            Console.WriteLine("Dash Offset      :      {0}", vrule8.dash_offset);
            Console.WriteLine("Serialization    :      {0}", vrule8.ToString());
        }
#endif
    }
}
