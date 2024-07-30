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

/////////////////////////////////////////////////////////////////////////////////////////////////
//File       : CDEF.cs   -- provides .Net / Mono wrapper around the RRDTool CDEF Construct     //
//Application: NHawk: Open Source Project Support                                              // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                 //
//Author     : Mike Corley, Syracuse University                                                //
//             mwcorley@syr.edu                                                                //
/////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides the class definition for the NHAWK CDEF construct, which constitutes a thin wrapper
 * around the rrdtool CDEF construct. It is intended that the public interface intuitively resemble native rrdtool 
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
 *   --exception handling and error reporting described where appropriate
 *  [Serializable]
 *  public class CDEF_Exception : Exception
 * 
 *  [Serializable]
 *  public class CDEF
 * 
 *  public CDEF(string vname, string rpn_expression
 *  -- constructor: accepts vname, and rpn_expression
 *     vname         :  the variable name for the CDEF
 *     rpn_expression:  the postfix mathematical expression 
 *     
 *  public string vname
 *  --  gets of sets the variable name for the CDEF
 *  
 *  public string rpn_expression
 *  -- gets or sets the postfix expression
 *  
 *  public CDEF(string cdef)
 *  -- constructor: constructs a CDEF object by deserializing a raw (serialized) CDEF in string
 *     format.  This is used by the GRAPH class for deserializing graphs as composite structures.
 *     formatting errors will result in a CDEFFormatException exception being thrown
 *  
 *  public override string ToString()
 *  -- serializes this instance to string representation
 *  
 *  public static CDEF DeepClone(CDEF cdef)
 *  -- returns a (new memory) copy of this CDEF inbstance. This is used by the GRAPH class for tracking list of CDEF 
 *     Note on DeepCloning: In the NHAWK model, the DeepClone works in the shallow reference model because every member
 *     member stored is either value type or string reference type and new string objects are construced, initialized and 
 *     returned with temporary StringBuilder() wiith which the ToString() method is called to yield the new string instance.
 *     This is useful be graph definitions won't be susceptible to potential modification by external defined references. 
 *     
 *  Example Usage
 *  =============
 *  See Test stub below
 * 
 *  Build Process
 *  =============
 *  
 * This module can be compiled stand alone for testing purposes: 
 * Note: some modules have additional assemblies dependencies which
 *       are needed for compilation of the test stub.  The required files section
 *       shows true dependency structure. i.e when the test stub is not compiled.
 *
 * Compiler Command:
 * Visual Studio 2008 (.Net 3.5): csc  /define:TEST_CDEF CDEF.cs 
 * Win32 Mono (1.9.1)           : gmcs -define:TEST_CDEF CDEF.cs 
 * SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_CDEF CDEF.cs 
 *                                                                  
 *  Required Files:   CDEF.cs
 *  
 *  Maintanence History
 *  ===================
 *  version 1.0 : 01 August 08
 *     -- first release: 
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace NHAWK
{
    
    [Serializable]
    public class CDEF_Exception : Exception
    {
        public CDEF_Exception(string message)
            : base(message)
        { }

        public CDEF_Exception(string message, Exception ex)
            : base(message, ex)
        { }
    };

    [Serializable]
    public class CDEF
    {
        const int TOKEN_COUNT = 2;
        string _vname;
        string _rpn_expression;
     
        
        public CDEF(string vname, string rpn_expression)
        {
            this.vname = vname;
            this.rpn_expression = rpn_expression;
        }


        public string vname
        {
            get { return _vname; }
            set { _vname = value; }
        }


        public string rpn_expression
        {
            get { return _rpn_expression; }
            set
            {
                if (value.Contains(" "))
                       value = value.Replace(" ", "");
                _rpn_expression = value;
            }
        }


        public CDEF(string cdef)
        {
            string[] top_level_tokens = cdef.Split(new char[] { ':' });

            if (top_level_tokens.Length != TOKEN_COUNT)
                throw new CDEF_Exception("Bad CDEF format");

            string[] inner_tokens = top_level_tokens[1].Split(new char[] { '=' });

            if (inner_tokens.Length != TOKEN_COUNT)
                throw new CDEF_Exception("Bad CDEF format");

            vname = inner_tokens[0];
            rpn_expression = inner_tokens[1];
        }


        public override string ToString()
        {
            return "CDEF:" + this.vname + "=" + rpn_expression;
        }

        public static CDEF DeepClone(CDEF cdef)
        {
            return new CDEF(((new StringBuilder()).Append(cdef.vname)).ToString(),
                             ((new StringBuilder()).Append(cdef.rpn_expression)).ToString());
        }

        #if TEST_CDEF
         public static void Main()
         {
             Console.WriteLine("\nTesting CDEF construction");
             CDEF cdef = new CDEF("mydatabits", "mydata,8,*");

             Console.WriteLine("\nCDEF: cdef1");
             Console.WriteLine("hashcode      : {0}", cdef.GetHashCode());
             Console.WriteLine("vname         : {0}", cdef.vname);
             Console.WriteLine("rpn expression: {0}", cdef.rpn_expression);
             Console.WriteLine("serialization : {0}", cdef.ToString());

             Console.WriteLine("\nTesting DeepClone");
             CDEF cdef2 = CDEF.DeepClone(cdef);

             Console.WriteLine("CDEF: cdef2");
             Console.WriteLine("hashcode      : {0}", cdef2.GetHashCode());
             Console.WriteLine("vname         : {0}", cdef2.vname);
             Console.WriteLine("rpn expression: {0}", cdef2.rpn_expression);
             Console.WriteLine("serialization : {0}", cdef2.ToString());

             Console.WriteLine("\nTesting Deserialization constructor");
             CDEF cdef3 = new CDEF(cdef2.ToString());
             Console.WriteLine("CDEF: cdef3");
             Console.WriteLine("hashcode      : {0}", cdef3.GetHashCode());
             Console.WriteLine("vname         : {0}", cdef3.vname);
             Console.WriteLine("rpn expression: {0}", cdef3.rpn_expression);
             Console.WriteLine("serialization : {0}", cdef3.ToString());
         }
        #endif 

    }
}

