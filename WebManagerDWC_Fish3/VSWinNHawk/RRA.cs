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

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//File       : RRA.cs   -- provides .Net / Mono wrapper around the RRDTool RRA (Round Robin Archive) construct //
//Application: NHawk: Open Source Project Support                                                              // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                                 //
//Author     : Mike Corley, Syracuse University                                                                //
//             mwcorley@syr.edu                                                                                //
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module is a C# abstraction that provides a .Net interface to the RRDTool RRA (Round Robin Archive) construct 
 * which is a component of the RRDTool Database structure.  This class seeks to encapsulate elements the 
 * in C# object model while retaining intuitive use of the RRDTool syntax. It also seeks to enforce code correctness
 * by enforcing proper use of constructs, and perserving RRDTool level relationships and dependenices between 
 * corresponding entities.  This is accomplished through semantically appropriate exception handling.  This class 
 * was developed with the intention of perserving the RRDTool syntax and semantics, enabling .Net developers to 
 * rapidy gain development productivity by refering directly to Tobi Oetiker's main RRDTool documentation page: 
 * http://oss.oetiker.ch/rrdtool/doc/rrdcreate.en.html
 *
 * 
 * Public Interface
 * ================
 * public RRA(CF cf, double xff, int steps, int rows)
 *   -- constructor:  accepts consolidation function (cf), (MIN, MAX, AVERAGE, LAST)
 *                    xfiles factor (xff),  number of samples that comprise the consolidation interval
 *                    (steps), and the number consolidation points to keep
 *                    which defines the overall resolution of the RRA (rows)
 *                    note: xff must be 0.0 => xff <= 1.0 or  an RRAFormatException will be thrown
 *                          steps and rows must > 0 or an RRAFormatException will be thrown
 *                    
 * public RRA(string rra)
 *    -- constructor: constructs an RRA object by deserializing an existing RRA from string format.  
 *                   This is used by the RRD class for deserializing graphs as composite 
 *                   structures.  if rra cannot be deserialized a RRAFormatException will be thrown 
 *                   
 * public static RRA DeepClone(RRA rra)
 *    -- returns a deep copy of the rra passed in. (note: this works becuase every NHawk class is capable serializing
 *       and deserializing it's state).  See my discussion on value model versus shallow reference model semantics.
 *       
 * public CF consolidationFunc
 *    -- sets or gets the consolidation function for the RRA (MAX, MIN, AVERAGE, LAST)
 *    
 * public int rows
 *    -- sets of gets the number consolidation points to keep which defines the overall 
 *       resolution of the RRA
 *       rows must be  > 0 or an RRAFormatException will be thrown
 *       
 * public int steps
 *    -- gets or sets the number of samples that comprise the consolidation interval
 *        steps must be > 0 or an RRA format exception will be thrown
 *    
 * public double xff
 *    --gets or sets the x files factor (determines what percentage of the database can consist of 
 *      unknown data values: must be (0.0 >= xff <= 1.0) or an RRAFormatException will be thrown
 *      
 * public override string ToString()
 *    -- serialize the object state to string
 *   
 * Build Process
 * =============
 * Compiler Command:  csc /define:TEST_RRA RRA.cs
 * Required Files  :  RRA.cs
 * 
 * Maintanence History
 * ===================
 * version 1.0 : 01 August 08 
 *      -- first release
 * 
*/

using System;

[assembly: CLSCompliant(true)]
namespace NHAWK
{
    [Serializable]
    public class RRAFormatException : Exception
    {
        public RRAFormatException(string message)
            : base(message)
        { }

        public RRAFormatException(string message, Exception ex)
            : base(message, ex)
        { }
    };
       
    [Serializable]
    public class RRA
    {
        public enum CF
        {
            AVERAGE, MIN, MAX, LAST, NA
        };

        private const double XFF_MAX = 1.0;
        private const double XFF_MIN = 0.0;
        private const int    TOKEN_COUNT = 5;
        private CF           _cf;
        private double       _xff;
        private int          _steps;
        private int          _rows;
   
        public RRA(CF cf, double xff, int steps, int rows)
        {
            this.consolidationFunc = cf;
            this.xff = xff;
            this.steps = steps;
            this.rows = rows;
        }

        public RRA(string rra)
        {
            //deserialize RAA 
            try
            {
                string[] rra_tokens = rra.Split(new char[] {':'} );

                if (rra_tokens.Length != TOKEN_COUNT)
                    throw new RRAFormatException("Bad RRA format");

                RemovePadding(rra_tokens);

                this.consolidationFunc = StringToCF(rra_tokens[1]);
                xff = Double.Parse(rra_tokens[2]);

                if(!(xff >= RRA.XFF_MIN && xff <= RRA.XFF_MAX))
                   throw new RRAFormatException("xfiles factor (xff) must range between 0 and 1");

                steps = int.Parse(rra_tokens[3]);
                rows = int.Parse(rra_tokens[4]);
            }
            catch(Exception ex)
            {
                if (ex is RRAFormatException)
                    throw;
                else
                   throw new RRAFormatException("could not deserialize RRA", ex);
            }
        }

        public static RRA DeepClone(RRA rra)
        {
            //all value types 
            return new RRA(rra.consolidationFunc, rra.xff, rra.steps, rra.rows);
        }

        public override string ToString()
        {
            return "RRA:" + consolidationFunc + ":" + xff + ":" + steps + ":" + rows;
        }


        public CF consolidationFunc
        {
            get { return _cf; }
            set { _cf = value; }
        }

        public int rows
        {
            get { return _rows; }
            set
            {
                if (value < 0)
                    throw new RRAFormatException("rows cannot be negative");

                _rows = value;
            }
        }

        public int steps
        {
            get { return _steps; }
            set
            {
                if (value < 0)
                    throw new RRAFormatException("steps cannot be negative");

                _steps = value;
            }

        }

        public double xff
        {
            get { return _xff; }
            set
            {
                if (!(value >= RRA.XFF_MIN && value <= RRA.XFF_MAX))
                    throw new RRAFormatException("xfiles factor (xff) must range between: " + RRA.XFF_MIN + " and " + RRA.XFF_MAX);
                
                _xff = value;
            }
        }

        private void RemovePadding(String[] vals)
        {
            for (int i = 0; i < vals.Length; i++)
                vals[i] = vals[i].Trim();
        }


        public static CF StringToCF(String cf)
        {
            switch (cf)
            {
                case "AVERAGE":
                    return CF.AVERAGE;
                case "MIN":
                    return CF.MIN;
                case "MAX":
                    return CF.MAX;
                case "LAST":
                    return CF.LAST;
                default:
                    throw new RRAFormatException("unknown consolidation function: " + cf);
            };
        }
     
        #if TEST_RRA
        public static void Main(string[] args)
        {
            Console.WriteLine("Testing RRA class");
            Console.WriteLine("=================");

            Console.WriteLine("Constructing RRA...");
            RRA rra1 = new RRA(RRA.CF.AVERAGE, 0.5, 1, 24); 
           
            Console.WriteLine("Consolidation Function: {0}", rra1.consolidationFunc.ToString());
            Console.WriteLine("X file factor         : {0}", rra1.xff);
            Console.WriteLine("Steps                 : {0}", rra1.steps);
            Console.WriteLine("Rows                  : {0}", rra1.rows);
            Console.WriteLine("Test Serialization    : {0}", rra1.ToString());

            Console.WriteLine("\nTesting Deserialization:");
            RRA rra2 = new RRA(rra1.ToString());
            Console.WriteLine("rra4 instance hash code: {0}", rra2.GetHashCode());
            Console.WriteLine("Consolidation Function : {0}", rra2.consolidationFunc.ToString());
            Console.WriteLine("X file factor          : {0}", rra2.xff);
            Console.WriteLine("Steps                  : {0}", rra2.steps);
            Console.WriteLine("Rows                   : {0}", rra2.rows);
            Console.WriteLine("Testing Serialization  : {0}", rra2.ToString());

            RRA rra3;
            try
            {
                Console.WriteLine("Constructing RRA... testing RRAFormatException for xff factor, should throw (needs to be between 0 and 1)");
                rra3 = new RRA(CF.AVERAGE, 1.5, 1, 24);
                Console.WriteLine("Consolidation Function : {0}", rra1.consolidationFunc.ToString());
                Console.WriteLine("X file factor          : {0}", rra1.xff);
                Console.WriteLine("Steps                  : {0}", rra1.steps);
                Console.WriteLine("Rows                   : {0}", rra1.rows);
                Console.WriteLine("Testing Serialization  : {0}", rra1.ToString());
            }
            catch (RRAFormatException ex)
            {
                Console.WriteLine("and it did: {0} " + ex.Message);
            }

            Console.WriteLine("\nTesting Deep cloning : (notice different hash codes for rra2 and rra4");
            RRA rra4 = RRA.DeepClone(rra2);

            Console.WriteLine("rra4 instance hash code: {0}", rra4.GetHashCode());
            Console.WriteLine("Consolidation Function : {0}", rra4.consolidationFunc.ToString());
            Console.WriteLine("X file factor          : {0}", rra4.xff);
            Console.WriteLine("Steps                  : {0}", rra4.steps);
            Console.WriteLine("Rows                   : {0}", rra4.rows);
            Console.WriteLine("Testing Serialization  : {0}", rra4.ToString());

            Console.WriteLine("\nTesting shallow copy (notice equal hash codes for rra4 and rra5");
            RRA rra5 = rra4;
            Console.WriteLine("rra5 instance hash code: {0}", rra5.GetHashCode());
            Console.WriteLine("Consolidation Function : {0}", rra5.consolidationFunc.ToString());
            Console.WriteLine("X file factor          : {0}", rra5.xff);
            Console.WriteLine("Steps                  : {0}", rra5.steps);
            Console.WriteLine("Rows                   : {0}", rra5.rows);
            Console.WriteLine("Testing Serialization  : {0}", rra5.ToString());

            Console.WriteLine("\nTesting accessors/mutators");
            rra5.consolidationFunc   = CF.LAST;
            rra5.rows = 100;
            rra5.steps = 20;
            rra5.xff = .75;
            Console.WriteLine("rra5 instance hash code: {0}", rra5.GetHashCode());
            Console.WriteLine("Consolidation Function : {0}", rra5.consolidationFunc.ToString());
            Console.WriteLine("X file factor          : {0}", rra5.xff);
            Console.WriteLine("Steps                  : {0}", rra5.steps);
            Console.WriteLine("Rows                   : {0}", rra5.rows);
            Console.WriteLine("Testing Serialization  : {0}", rra5.ToString());

            try
            {
                Console.WriteLine("\nTesting RRAFormatException propagation/handler for mutators properties");
                rra5.consolidationFunc = CF.LAST;
                //uncomment the fields below to test
                //rra5.rows = -1;
                //rra5.steps = -1;
                  rra5.xff = 1.1;
            }
            catch (RRAFormatException ex)
            {
               Console.WriteLine("In exception handler: {0}", ex.Message);
            }
        }
        #endif 
    }
}
