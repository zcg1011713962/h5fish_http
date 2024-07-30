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
//File       : SHIFT.cs   -- .Net / Mono RRDTool SHIFT element class                                                            //
//Application: NHawk: Open Source Project Support                                                             // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                                //
//Author     : Mike Corley, Syracuse University                                                               //
//             mwcorley@syr.edu                                                                               //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides the class definition for the NHAWK SHIFT construct which constitutes a thin wrapper
 * around the rrdtool SHIFT graph element construct. It is intended that the public interface intuitively resemble 
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
 * RRDtoool docs :=> SHIFT:vname:offset
 * Using this command RRDtool will graph the following elements with the specified offset. 
 * For instance, you can specify an offset of ( 7*24*60*60 = ) 604'800 seconds to "look back" 
 * one week. Make sure to tell the viewer of your graph you did this ... \
 * As with the other graphing elements, you can specify a number or a variable here.
 *
 * [Serializable]
 * public class SHIFT : AGRAPH_ELEMENT 
 * 
 * public SHIFT(string vname, int offset)
 * -- constructor: accepts a variable name or value, and the offset
 *    vname : the variable or value 
 *    offset: tells RRDtool where in the data to begin graphing
 *    
 * public SHIFT(string shift)
 * -- deserializes and constructs a serialized (string) SHIFT representation
 * 
 * public string vname
 * -- gets or sets the variable or value
 * 
 * public int offset
 * -- gets or sets the offset
 * 
 *  public IGRAPH_ELEMENT DeepClone()
 *    -- deep clone override: polymorphically returns a (new memory) copy of the SHIFT element. 
 *       This is used by the GRAPH class for adding elements to the element list for tracking.  
 *       Note on DeepCloning: In the NHAWK model, the DeepClone works in the shallow reference model 
 *       because every member member stored is either value type or string reference type and new string 
 *       objects are construced, initialized and returned with temporary StringBuilder(), with it's ToString() 
 *       method is called to yield the new string instance. This is useful because graph definitions won't be 
 *       susceptible to potential modification by external defined references.
 *       
 *  public override string ToString()
 *     -- serialization override:  serializes the SHIFT element to a string.  The is called polymorphically 
 *        by the GRAPH class which will ask each graph element (contained within its definition) to serialize 
 *        itself so the entire GRAPH structure (as a composite structure), can be serialized and sent to rrdtool
 *        executable for processing
 *        
 *  public static SHIFT DeepClone(SHIFT shift)
 *   -- provides the same operation as "IGRAPH_ELEMENT DeepClone()", but doesn't work polymorphically.  
 *      This is generally the preferred version that would be called directly by clients.
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
 * Visual Studio 2008 (.Net 3.5): csc /define:TEST_SHIFT_ELEMENT /reference:c:\windows\microsoft.net\framework\v2.0.50727\Microsoft.VisualBasic.dll 
 *                                   ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs SHIFT.cs  
 *    
 * Win32 Mono (1.9.1)           : gmcs -define:TEST_SHIFT_ELEMENT -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.Drawing.dll 
 *                                                                -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\Microsoft.VisualBasic.dll
 *                                                                -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.dll 
 *                                                                ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs SHIFT.cs
 *                                                                   
 * SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_SHIFT_ELEMENT -r:/usr/lib/mono/gac/System/2.0.0.0__b77a5c561934e089/System.dll 
 *                                                                -r:/usr/lib/mono/gac/System.Drawing/2.0.0.0__b03f5f7f11d50a3a/System.Drawing.dll 
 *                                                                -r:/usr/lib/mono/gac/Microsoft.VisualBasic/8.0.0.0__b03f5f7f11d50a3a/Microsoft.VisualBasic.dll  
 *                                                                ../GRAPH_ELEMENT/GRAPH_ELEMENT.cs SHIFT.cs                                              
 *                                                                  
 * Required Files:   SHIFT.cs GRAPH_ELEMENT.cs
 * 
 * Maintanence History
 * =================== 
 * version 1.0 : 01 Aug 08 
 *    -- first release 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace NHAWK
{
    public class SHIFT : IGRAPH_ELEMENT
    {
        private string _vname;
        private int    _offset;

        public string vname
        {
            get { return _vname; }
            set { _vname = value; }
        }

        public int offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public SHIFT(string vname, int offset)
        {
            this.vname = vname;
            this.offset = offset;
        }

        public SHIFT(string shift)
        {
            string[] args = shift.Split(new char[] { ':' });
            vname = args[1];
            offset = int.Parse(args[2]);
        }

        public IGRAPH_ELEMENT DeepClone()
        {
            return new SHIFT( ((new StringBuilder()).Append(vname)).ToString(), offset);
        }

        public static SHIFT DeepClone(SHIFT shift)
        {
            return new SHIFT(((new StringBuilder()).Append(shift.vname)).ToString(), shift.offset);
        }

        public override string ToString()
        {
            return "SHIFT:" + vname + ":" + offset;
        }
       
        #if TEST_SHIFT_ELEMENT
        public static void Main()
        {
            Console.WriteLine("\nTesting Shift class");
            SHIFT shift1 = new SHIFT("test_shift1", 10);
            Console.WriteLine("vname : {0} ", shift1.vname);
            Console.WriteLine("offset: {0} ", shift1.offset);

            Console.WriteLine("\nTesting DeepClone");
            SHIFT shift2 = SHIFT.DeepClone(shift1);
            Console.WriteLine("vname : {0} ", shift2.vname);
            Console.WriteLine("offset: {0} ", shift2.offset);

            Console.WriteLine("\nTesting deserialization ctor");
            SHIFT shift3 = new SHIFT(shift2.ToString());
            Console.WriteLine("vname : {0} ", shift2.vname);
            Console.WriteLine("offset: {0} ", shift2.offset);
        }
       #endif 
    }
}
