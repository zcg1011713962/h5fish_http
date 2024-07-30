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

///////////////////////////////////////////////////////////////////////////////////////////////////////////
//File       : DS.cs   --  C# abstraction for RRDTool DS (Data Source) construct                         //
//Application: NHawk: Open Source Project Support                                                        // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                           //
//Author     : Mike Corley, Syracuse University                                                          //
//             mwcorley@syr.edu                                                                          //
///////////////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides a C# .Net interface to the RRDTool DS (Data Source) component of the RRDTool database 
 * structure.  This class seeks to encapsulate the DS semantics in the C# .Net object model, while retaining
 * intuitive structure of RRDTool syntax. The DS class is equipt with appropriate exception (error) handling 
 * for enforces both .Net code correctness and semantically use related RRDTool constructs. The goal of the 
 * NHawk project is to provide the RRDTool facility (natively) in the .Net paradigm while promoting loose coupling
 * between the two paradigms and preserving RRDTool syntax and semantics.  This help enable .Net developers to rapidy 
 * gain development productivity, in that usability (for the more part) be infered intuitively for the RRDTool doc.
 * Note: the manual page described herein while NOT seek to explain, disclose, or exemplify the
 * RRDTool construct semantics and typical usage scenarios.  That is done quite well by the RRDTool author 
 * (Tobi Oetiker), and his supporters.  The goal of the NHawk project is to allow .Net / Mono (C#) developers 
 * to leverage RRDTool natively from .Net by refering (intuitively) directly to the RRDTool main doc. page:
 * http://oss.oetiker.ch/rrdtool/doc/rrdcreate.en.html.
 * 
 * We will however, provide as many examples, and tutorials as time allows, and depending on the success of the 
 * project, we hope a large user base will contribute to ehancements, tutorials, and general support.  We feel 
 * the HNawk is gret way to benefit .Net and Mono FCL (which currently lacks support for time series visualization), 
 * and RRDTool by unleasing its potential by integrating it with the abundance and power of the .Net FCL. 
 * 
 * 
 * Public Interface
 * ================
 * 
 * public DS(string n, TYPE t)
 *    -- constructs a basic data source (DS) entity, accepts name, and TYPE (GAUGE, COUNTER, DERIVE, etc.)
 *       sets default heartbeat=600 secs., min=Unknown, max=Unknown
 *    
 * public DS(string name, TYPE t, long heart_beat, double min, double max)
 *    -- constructs a fully defined data source (DS) entity
 *    
 * public DS(string data_source)
 *    -- constructs a fully defined data source (DS) entity from a serialized DS string
 *       note: if data_source cannot be deserialzed a DSFormatException() is thrown
 * 
 * public TYPE type
 *    -- gets or sets the type of the data source: GAUGE, COUNTER, DERIVE, ABSOLUTE 
 *    
 * public static double U        
 *    -- gets Unknown limit specifier (for min and max values of an DS)
 *    
 * public double heartbeat
 *    -- gets or sets the heartbeat... 
 *     *** heartbeat defines the maximum number of seconds that may pass between 
 *         updates of this data source before the value of the data source is assumed to be *UNKNOWN* 
 *         Source document: http://oss.oetiker.ch/rrdtool/doc/rrdcreate.en.html 
 * 
 * public double min       
 *    -- gets or sets the min range value
 *    *** min and max define the expected range values for data supplied by a data source. 
 *        If min and/or max any value outside the defined range will be regarded as *UNKNOWN*. 
 *        If you do not know or care about min and max, set them to U for unknown. 
 *        Note that min and max always refer to the processed values of the DS. 
 *        Source document: http://oss.oetiker.ch/rrdtool/doc/rrdcreate.en.html 
 * 
 * public double max
 *    -- gets or sets the max range value (see discussion for min)
 *        
 * Example Usage
 * =============
 * See Test stub below
 * 
 * Build Process
 * ============= 
 * This module can be compiled stand alone for testing purposes: 
 * Note: some modules have additional assembly dependencies which
 * are needed for compilation of the test stub.  The required files section
 * shows true dependency structure. i.e when the test stub is not compiled.
 *
 * Compiler Command:
 * Visual Studio 2008 (.Net 3.5): csc  /define:TEST_DS  DS.cs 
 * Win32 Mono (1.9.1)           : gmcs -define:TEST_DS  DS.cs 
 * SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_DS  DS.cs 
 *                                                                  
 * Required Files: DS.cs
 *
 * Maintanence History
 * ===================
 * version 1.0 : 01 August 08 
 *     -- first release: (no support for COMPUTE DS type)
 *     
*/

using System;
using System.Text;

namespace NHAWK
{
    [Serializable]
    public class DSFormatException : Exception
    {
        public DSFormatException(string message)
            : base(message)
        { }

        public DSFormatException(string message, Exception ex)
            : base(message, ex)
        { }
    };
 
    [Serializable]
    public class DS
    {
        private const long  DEF_HEARTBEAT = 600;
        private const int   MAX_LEN = 19;
        private const int   MAX_TOKEN_COUNT = 6;
        private string     _name;
        private TYPE       _type;
        private long       _heart_beat;
        private double     _min;
        private double     _max;
        
        //--< Data source types as defined by RRDTool >--
        public enum TYPE
        {
            GAUGE=0, COUNTER=1, DERIVE=2, ABSOLUTE=3
        };

        //--< Unknown limit specifier (min and max values of an DS) >--
        public static double U
        {
           get {return Double.NaN;}
        }

        //--<constructs a basic data source (DS) entity >--
        public DS(string n, TYPE t)
        {
            name = n;
            DSType = t;
            heartbeat = DEF_HEARTBEAT;
            min = double.NaN;
            max = double.NaN;
        }

        //--<constructs a fully defined data source (DS) entity >--
        public DS(string name, TYPE t, long heart_beat, double min, double max): this(name, t)
        {
            this.heartbeat = heart_beat;
            this.min = min;
            this.max = max;
        }

        //--<constructs a fully defined data source (DS) entity from a serialized DS string >--
        public DS(string data_source)
        {
            //deserializes DS here
            try
            {
                string[] ds_tokens = data_source.Split(new char[] { ':' });
                if (ds_tokens.Length != MAX_TOKEN_COUNT)
                    throw new DSFormatException("Bad DS format");

                //remove any whitespace (shouldn't be any)
                RemovePadding(ds_tokens);
                name = ds_tokens[1];
                _type = StringToType(ds_tokens[2]);
                heartbeat = long.Parse(ds_tokens[3]);
                min = (ds_tokens[4] != "U") ? Double.Parse(ds_tokens[4]) : Double.NaN;
                max = (ds_tokens[5] != "U") ? Double.Parse(ds_tokens[5]) : Double.NaN;
            }
            catch (Exception ex)
            {
                if (ex is DSFormatException)
                    throw;
                else
                    throw new DSFormatException("bad data source format:  could not deserialize: " + data_source, ex);
            }
        }

        //--< get or set the type of the data source: GAUGE, COUNTER DERIVE, etc >--
        public TYPE DSType
        {
            get { return _type; }
            set { _type = value; }
        }

        //--< get or set the heartbeat... 
        /*  heartbeat defines the maximum number of seconds that may pass between 
            two updates of this data source before the value of the data source is assumed to be *UNKNOWN* 
            Source document: http://oss.oetiker.ch/rrdtool/doc/rrdcreate.en.html 
         */
        public long heartbeat
        {
            get { return _heart_beat; }
            set
            {
                if (value < 0)
                    throw new DSFormatException("HeartBeat cannot be negative");
                
                _heart_beat = value;
            }
        }

       /* --< get or set the min range value >--
        * min and max define the expected range values for data supplied by a data source. 
        * If min and/or max any value outside the defined range will be regarded as *UNKNOWN*. If you do not know or care about min and max, 
        * set them to U for unknown. Note that min and max always refer to the processed values of the DS. 
        * Source document: http://oss.oetiker.ch/rrdtool/doc/rrdcreate.en.html 
        */
        public double max
        {
            get { return _max; }
            set
            {
                //if (value == Double.NaN ||  value > _min)
                    _max = value;    
            }
        }
        
        //--< get or set the max range value >--
        public double min
        {
            get { return _min; }
            set
            {
                //if (value == Double.NaN || value < _min)
                    _min = value;
            }
        }

        // --< get or set the DS name >--
        public string name
        {
            get { return _name; }
            set
            {
                if (check_format(value.Trim()))
                    _name = value.Trim();
                else
                    throw new DSFormatException("Bad name for RRD Data Source: use [a-zA-Z0-9_] up to " + MAX_LEN.ToString() + " characters");
            }
        }

        // --< performs and returns a deep clone of the ds object passed .e.g. a completely different oject on the managed heap >-- 
        public static DS DeepClone(DS ds)
        {
            return new DS((new StringBuilder().Append(ds._name)).ToString(), ds.DSType, ds.heartbeat, ds.min, ds.max);
        }

        // --< serializes the data source to a string >--
        public override string ToString()
        {
            string local_min = (min.ToString().ToLower() != "nan") ? min.ToString() : "U";
            string local_max = (max.ToString().ToLower() != "nan") ? max.ToString() : "U";
        
            return "DS:" + name + ":" + DSType.ToString() + ":" + heartbeat + ":" + local_min + ":" + local_max;
        }

        private void RemovePadding(String[] vals)
        {
            for (int i = 0; i < vals.Length; i++)
                vals[i] = vals[i].Trim();
        }

        public static TYPE StringToType(String type)
        {
            switch (type)
            {
                case "COUNTER":
                    return TYPE.COUNTER;
                case "ABSOLUTE":
                    return TYPE.ABSOLUTE;
                case "DERIVE":
                    return TYPE.DERIVE;
                case "GAUGE":
                    return TYPE.GAUGE;
                default:
                    throw new DSFormatException("Bad DS type: " + type);
            };
        }

        private bool check_format(string val)
        {
            if (val.Length > MAX_LEN || val.Length == 0)
                return false;

            foreach (char ch in val)
                if (!Char.IsLetter(ch) && !Char.IsDigit(ch) && !(ch == '_'))
                    return false;

            return true;
        }

#if TEST_DS
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Data Source class");
            Console.WriteLine("=========================");

            Console.WriteLine("Constructing ds1 (simple construction)");
            DS ds1 = new DS("bandwidth", DS.TYPE.COUNTER);

            Console.WriteLine("Name         : {0}", ds1.name);
            Console.WriteLine("Type         : {0}", ds1.DSType);
            Console.WriteLine("Heartbeart   : {0}", ds1.heartbeat);
            Console.WriteLine("Max value    : {0}", ds1.max);
            Console.WriteLine("Min value    : {0}", ds1.min);
            Console.WriteLine("Serialization: {0}", ds1.ToString());

            Console.WriteLine("\nConstructing ds2... (complete construction)");
            DS ds2 = new DS("speed", DS.TYPE.GAUGE, 300, -34.5, 212.0);

            Console.WriteLine("Name         : {0}", ds2.name);
            Console.WriteLine("Type         : {0}", ds2.DSType);
            Console.WriteLine("Heartbeart   : {0}", ds2.heartbeat);
            Console.WriteLine("Max value    : {0}", ds2.max);
            Console.WriteLine("Min value    : {0}", ds2.min);
            Console.WriteLine("Serialization: {0}", ds2.ToString());

            try
            {
                Console.WriteLine("\nConstructing ds3 *** Testing DSFormatException for illegal name");
                DS ds3 = new DS("speed$", DS.TYPE.ABSOLUTE, 600, 0, DS.U);
            }
            catch (DSFormatException ex)
            {
                Console.WriteLine("In exception handler: {0}", ex.Message);
            }

            Console.WriteLine("\nTest Deserialization:");
            DS ds4 = new DS(ds2.ToString());
            Console.WriteLine("ds4 instance hash code: {0}", ds4.GetHashCode());
            Console.WriteLine("Name         : {0}", ds4.name);
            Console.WriteLine("Type         : {0}", ds4.DSType);
            Console.WriteLine("Heartbeart   : {0}", ds4.heartbeat);
            Console.WriteLine("Max value    : {0}", ds4.max);
            Console.WriteLine("Min value    : {0}", ds4.min);
            Console.WriteLine("Serialization: {0}", ds4.ToString());

            Console.WriteLine("\nTesting deep cloning... ds5(ds4)  notice different codes for ds4 and ds5");
            Console.WriteLine("\nTesting mutators: i.e. ds4 and ds5 will have different values");
            DS ds5 = DS.DeepClone(ds4);

            ds5.name = "new_speed";
            ds5.DSType = DS.TYPE.COUNTER;
            ds5.heartbeat = 120;
            ds5.min = -1.5;
            ds5.max = 2.5;

            Console.WriteLine("ds5 instance hash code: {0}", ds5.GetHashCode());
            Console.WriteLine("Name         : {0}", ds5.name);
            Console.WriteLine("Type         : {0}", ds5.DSType);
            Console.WriteLine("Heartbeart   : {0}", ds5.heartbeat);
            Console.WriteLine("Max value    : {0}", ds5.max);
            Console.WriteLine("Min value    : {0}", ds5.min);
            Console.WriteLine("Serialization: {0}", ds5.ToString());

            Console.WriteLine("Testing exception handling with mutator properties"); 
            try
            {
                //ds5.name = "new_speed$_";
                ds5.DSType = DS.TYPE.COUNTER;
                ds5.heartbeat = -1;
                ds5.min = -1.5;
                ds5.max = 2.5;
            }
            catch (DSFormatException ex)
            {
                Console.WriteLine("In exception handler: {0} ", ex.Message);
            }

            Console.WriteLine("\nFinally...  this is shallow copy (notice hashcodes are same for ds4 and ds6}");
            DS ds6 = ds4;

            Console.WriteLine("ds6 instance hash code: {0}", ds6.GetHashCode());
            Console.WriteLine("Name         : {0}", ds6.name);
            Console.WriteLine("Type         : {0}", ds6.DSType);
            Console.WriteLine("Heartbeart   : {0}", ds6.heartbeat);
            Console.WriteLine("Max value    : {0}", ds6.max);
            Console.WriteLine("Min value    : {0}", ds6.min);
            Console.WriteLine("Serialization: {0}", ds6.ToString());
        }
#endif
    }
}
