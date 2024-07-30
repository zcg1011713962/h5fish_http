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

//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//File       : DEF.cs   -- provides .Net / Mono wrapper around the RRDTool Graph Definition (DEF) Construct //
//Application: NHawk: Open Source Project Support                                                           // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                              //
//Author     : Mike Corley, Syracuse University                                                             //
//             mwcorley@syr.edu                                                                             //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides the class definition for the NHAWK DEF construct, which constitutes a thin wrapper
 * around the rrdtool DEF construct (fetches data from an RRD). It is intended that the public interface 
 * intuitively resemble native rrdtool construct syntax. For usage detail see this interface and rrdtool man page 
 * documentation at: http://oss.oetiker.ch/rrdtool/doc/rrdgraph_data.en.html
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
 * 
 *   -- exception handling and error reporting facilities
 *      usage described below where appropriate
 *   [Serializable]
 *   public class DataSourceException : Exception
 * 
 *   [Serializable]
 *   public class RRACFException : Exception
 *   
 *   [Serializable]
 *   public class RRACFException : Exception
 *  
 *   [Serializable]
 *   public class DEF
 * 
 *   public DEF(string vname, RRD rrd, string ds_name, RRA.CF cf)
 *     --  constructor: accepts graph definition name (vname), an RRD Database file (rrd),
 *         and the data source (ds_name) present in the rrd), and the consolidation function (cf)
 *         corresponding to the RRA definition in the rrd.
 *         vname   :  must exist in the "rrd" definition or a DataSourceException will be thrown 
 *         rrd     :  a valid rrd to read from
 *         ds_name :  must refer to a vaild data source in the "rrd" or a DefException will be thrown
 *         cf      :  must correspond to an RRA definition in "rrd" or a n RRACFException will be thrown 
 *         
 *   public DEF(string vname, string rrd_filename, string ds_name, RRA.CF cf) 
 *   --  constructor: accepts graph definition name (vname), an RRD Database file name: e.g. "test.rrd",
 *       and the data source (ds_name) present in the rrd), and the consolidation function (cf)
 *       corresponding to the RRA definition in the rrd.
 *       vname        : must not have any spaces or tabs or DefException will be thrown
 *       rrd_filename : name of a valid rrd for reading
 *       ds_name      : must reference a valid data source in the "rrd" definition or a DataSourceException will be thrown 
 *       cf           : must correspond to an RRA definition in "rrd" or a n RRACFException will be thrown 
 *       
 *   public DEF(string vname, string rrd_filename, string ds_name, RRA.CF cf, long step)     
 *    --  constructor: accepts graph definition name (def_name), an RRD Database file name: e.g. "test.rrd",
 *        and the data source (ds_name) present in the rrd), and the consolidation function (cf)
 *        corresponding to the RRA definition in the rrd.
 *        vname        : must not have any spaces or tabs or DefException will be thrown
 *        rrd_filename : name of a valid rrd for reading
 *        ds_name      : must reference a valid data source in the "rrd" definition or a DataSourceException will be thrown 
 *        cf           : must correspond to an RRA definition in "rrd" or a n RRACFException will be thrown 
 *        step         : specifies a  manual override to for chosing a specific RRA with the desired resolution         
 *     
 *   public DEF(string vname, string rrd_filename, string ds_name, RRA.CF cf, long step, string start, string stop, RRA.CF reduce)
 *   --  constructor: accepts graph definition name (def_name), an RRD Database file name: e.g. "test.rrd",
 *       and the data source (ds_name) present in the rrd), and the consolidation function (cf)
 *       corresponding to the RRA definition in the rrd.
 *       vname        : must not have any spaces or tabs or a DefException will be thrown
 *       rrd_filename : name of a valid rrd for reading
 *       ds_name      : must reference a valid data source in the "rrd" definition or a DataSourceException will be thrown 
 *       cf           : must correspond to an RRA definition in "rrd" or a n RRACFException will be thrown 
 *       step         : specifies a  manual override to for chosing a specific RRA with the desired resolution    
 *       start and end: speficify the timespan to be graphed (default == timespan of data in RRD) 
 *       
 *   public DEF(string vname, RRD rrd_filename, string ds_name, RRA.CF cf, long step, string start, string stop, RRA.CF reduce)
 *   --  constructor: accepts graph definition name (def_name), an RRD Database
 *       and the data source (ds_name) present in the rrd), and the consolidation function (cf)
 *       corresponding to the RRA definition in the rrd.
 *       vname        : must not have any spaces or tabs or a DefException will be thrown
 *       rrd_filename : name of a valid rrd for reading
 *       ds_name      : must reference a valid data source in the "rrd" definition or a DataSourceException will be thrown 
 *       cf           : must correspond to an RRA definition in "rrd" or a n RRACFException will be thrown 
 *       step         : specifies a  manual override to for chosing a specific RRA with the desired resolution    
 *       start and end: speficify the timespan to be graphed (default == timespan of data in RRD) 
 * 
 *   public DEF(string def)
 *     -- constructor: constructs a DEF object by deserializing an existing graph definition from a string
 *        format.  This is used by the GRAPH class for deserializing graphs as composite structures.
 *        formatting errors will result in the appropriate exception being thrown
 *        
 *   public string name   
 *     -- gets or sets the def name
 *     
 *   public string ds     
 *     -- gets or sets the data source referenced in the rrd file passed
 *     
 *   public RRA.CF cf     
 *     -- gets or sets the considation function referencing an RRA in rrd file passed
 *     
 *   public long step
 *     -- gets or sets the step for specifying RRA of specific resolution
 *     
 *   public string start
 *   public string stop 
 *     -- gets or sets the starting / ending timespan for the data to be graphed
 *     
 *   public static DEF DeepClone(DEF def) 
 *     -- returns a (new memory) copy of this DEF instance . This is used by the GRAPH class for tracking list of DEF 
 *        Note on DeepCloning: In the NHAWK model, the DeepClone works in the shallow reference model because every member
 *        member stored is either value type or string reference type and new string objects are construced, initialized and 
 *        returned with temporary StringBuilder() with which the ToString() method is called to yield the new string instance.
 *        This is useful be graph definitions won't be susceptible to potential modification by external defined references.   
 * 
 *   public override string ToString()
 *     -- serializes this instance to the string representation
 * 
 * Example Usage
 * =============
 * See Test stub below
 * 
 * Build Process
 * ============= 
 * This module can be compiled stand alone for testing purposes: 
 * Note: some modules have additional assemblies dependencies which
 *       are needed for compilation of the test stub.  The required files section
 *       shows true dependency structure. i.e when the test stub is not compiled.
 *
 * Compiler Command:
 * Visual Studio 2008 (.Net 3.5): csc  /define:TEST_GRAPH_DEF DEF.cs ..\DS\DS.cs ..\RRA\RRA.cs ..\RRD\RRD.cs ..\NHawkCommand\NHawkCommand.cs	
 * Win32 Mono (1.9.1)           : gmcs -define:TEST_GRAPH_DEF DEF.cs ..\DS\DS.cs ..\RRA\RRA.cs ..\RRD\RRD.cs ..\NHawkCommand\NHawkCommand.cs
 * SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_GRAPH_DEF DEF.cs ../DS/DS.cs ../RRA/RRA.cs ../RRD/RRD.cs ../NHawkCommand/NHawkCommand.cs
 *                                                                  
 * Required Files:   DEF.cs DS.cs RRA.cs RRD.cs HHawkCommand.cs
 * 
 * Maintanence History
 * ===================
 * version 1.0 : 01 August 08 
 *     -- first release: 
*/

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace NHAWK
{
    [Serializable]
    public class DataSourceException : Exception
    {
        public DataSourceException(string message)
            : base(message)
        { }

        public DataSourceException(string message, Exception ex)
            : base(message, ex)
        { }
    };

    [Serializable]
    public class DefException : Exception
    {
        public DefException(string message)
            : base(message)
        { }

        public DefException(string message, Exception ex) :
            base(message, ex)
        { }
    };

   
    [Serializable]
    public class RRACFException : Exception
    {
        public RRACFException(string message)
            : base(message)
        { }

        public RRACFException(string message, Exception ex)
            : base(message, ex)
        { }
    };

     [Serializable]
     public class DEF
     {
         const int MAX_TOKEN_COUNT = 7;
         const int MIN_TOKEN_COUNT = 4;
         
         string _def_name = "";
         string _ds_name = "";
         RRA.CF _cf;
         RRD    _rrd = null;
         long   _step;
         string _start;
         string _stop;
         RRA.CF _reduce;
        
         public DEF(string def_name, RRD rrd, string ds_name, RRA.CF cf)
         {
             bool cf_exists = false;
             
             if(!File.Exists(rrd.filename))
                 throw new RRDFileNotFoundException("RRD File: " + rrd.filename + " not found");

             if(rrd.ds_lookup(ds_name) == null)
                 throw new DataSourceException("DS: " + ds_name + " not found in RRD:" + ds_name);

             for (int i = 0; i < rrd.rra_count; i++)
                 if (rrd.rra_lookup(i).consolidationFunc == cf)
                     cf_exists = true;
     
             if(!cf_exists)
               throw new RRACFException("RRD File: " + rrd.filename + " does not have an RRA of type: " + cf.ToString());
         
             this.name = def_name;
             this.ds = ds_name;
             this.cf = cf;
             this._rrd = rrd;
             this.step = 0;
             this.start = "";
             this.stop  = "";
             this.reduce = RRA.CF.NA;
         }

         public DEF(string def_name, string rrd_filename, string ds_name, RRA.CF cf) 
             : this(def_name, RRD.load(rrd_filename), ds_name, cf)
         {}

         public DEF(string def_name, string rrd_filename, string ds_name, RRA.CF cf, long step)
             : this(def_name, rrd_filename, ds_name, cf)
         {
             this.step = step;
         }

         public DEF(string def_name, string rrd_filename, string ds_name, RRA.CF cf, long step, string start, string stop, RRA.CF reduce)
             : this(def_name, rrd_filename, ds_name, cf, step)
         {
             this.start = start;
             this.stop = stop;
             this.reduce = reduce;
         }

         public DEF(string def_name, RRD rrd_filename, string ds_name, RRA.CF cf, long step, string start, string stop, RRA.CF reduce)
             : this(def_name, rrd_filename, ds_name, cf)
         {
             this.step = step;
             this.start = start;
             this.stop = stop;
             this.reduce = reduce;
         }
  
         public static DEF DeepClone(DEF def)
         {
             return new DEF(  (new StringBuilder().Append(def.name)).ToString(),
                              RRD.DeepClone(def._rrd),
                              (new StringBuilder().Append(def.ds)).ToString(),
                              def.cf,
                              def.step,
                              (new StringBuilder().Append(def.start)).ToString(),
                              (new StringBuilder().Append(def.stop)).ToString(),
                              def.reduce);
         }


         private List<int> parser_index_list(string def)
         {
             string d = def.ToLower();
             List<int> index_list = new List<int>();

             int start_index = d.IndexOf("start=");
             for (int i = start_index; ((i >= 0) && (i < d.Length-1)); i++)
                 if ((d[i] == '\\') && (d[i+1] == ':'))
                     if (!index_list.Contains(i+1))
                         index_list.Add(i+1);

             int end_index = d.IndexOf("end=");
             for (int i = end_index; ((i >= 0) && (i < d.Length-1)); i++)
                 if ((d[i] == '\\') && (d[i + 1] == ':'))
                     if (!index_list.Contains(i+1))
                         index_list.Add(i+1);

             return index_list;
         }

         private string PreSerializeFormat(StringBuilder def, List<int> index_list)
         {
             foreach (int index in index_list)
                 def[index] = '_';

             return def.ToString();
         }

         private void PostSerializeFormat(StringBuilder start_, StringBuilder stop_)
         {

             for (int i = 0; i < start_.Length; i++)
                 if (start_[i] == '_')
                     start_[i] = ':';

             start = start_.ToString();


             for (int i = 0; i < stop_.Length; i++)
                 if (stop_[i] == '_')
                     stop_[i] = ':';

             stop = stop_.ToString();
         }


         private void argument_closure(string[] args, int i)
         {
             for (int j = i; j < args.Length; j++)
             {
                 string[] inner_args = args[j].Split(new char[] { '=' });

                 if (inner_args.Length != 2)
                     throw new DefException("bad graph syntax: expecting \"=\"");

                 switch (inner_args[0].Trim().ToLower())
                 {
                     case "step":
                         step = long.Parse(inner_args[1]);
                     break;

                     case "start":
                         start = inner_args[1];
                     break;

                     case "end":
                         stop = inner_args[1];
                     break;

                     default:
                     reduce = RRA.StringToCF(inner_args[1]);
                     break;
                    
                 };
             }
         }

         public DEF(string def)
         {
             //deserialize DEF 
             try
             {
                 //get list of indixces to fix parser
                 List<int> index_list = parser_index_list(def);
                 string pre_def = PreSerializeFormat(new StringBuilder(def), index_list);

                 step = 0;
                 start = stop = "";
                 reduce = RRA.CF.NA;
                 bool cf_exists = false;

                 string[] def_tokens = pre_def.Split(new char[] { ':' });

                 if (def_tokens.Length > MAX_TOKEN_COUNT && def_tokens.Length < MIN_TOKEN_COUNT)
                     throw new DefException("bad DEF syntax");
                 
                 RemovePadding(def_tokens);

                 string[] inner_tokens = def_tokens[1].Split(new char[] { '=' });

                 if (inner_tokens.Length != 2)
                     throw new DefException("bad DEF syntax at \"=\"");

                 this.name = inner_tokens[0];
                 this._rrd = RRD.load(inner_tokens[1]);
               
                 if (_rrd.ds_lookup(def_tokens[2]) == null)
                     throw new DataSourceException("DS: " + def_tokens[2] + " not found in RRD:" + _rrd.filename);

                 for (int i = 0; i < _rrd.rra_count; i++)
                     if (_rrd.rra_lookup(i).consolidationFunc == cf)
                         cf_exists = true;

                 if (!cf_exists)
                     throw new RRACFException("RRD File: " + _rrd.filename + " does not have an RRA of type: " + cf.ToString());

                 this.ds = def_tokens[2];
                 this.cf = RRA.StringToCF(def_tokens[3]);

                 if (def_tokens.Length > MIN_TOKEN_COUNT)
                     argument_closure(def_tokens, MIN_TOKEN_COUNT);

                 PostSerializeFormat(new StringBuilder(start), new StringBuilder(stop));
             }
             catch (Exception ex)
             {
                 if ( (ex is RRDFileNotFoundException) ||
                      (ex is RRDFileExistsException)   ||
                      (ex is RRDFormatException)  ||
                      (ex is RRAFormatException)  ||
                      (ex is DSFormatException)   ||
                      (ex is DataSourceException) ||
                      (ex is DefException)        ||
                      (ex is RRACFException))
                     throw;
                 else
                     throw new DefException("Bad DEF format", ex);
             }
         }
         

         public long step
         {
           get { return _step; }
           set 
           {
                  if(value < 0)
                    throw new DefException("step for cannot be negative");
               _step = value; 
           }
         }
       
       
         public string name
         {
             get { return _def_name; }
             set
             {
                if (!check_format(value.Trim()))
                     throw new DefException("Definition names must not have spaces or tabs");

                 _def_name = value.Trim();
             }
         }

         public string start
         {
             get { return _start; }
             set { _start = value; }
         }

         public string stop
         {
             get { return _stop; }
             set { _stop = value; }
         }

         public string ds
         {
             get { return _ds_name; }
             set { _ds_name = value.Trim(); }
         }


         public RRA.CF cf
         {
             get { return _cf; }
             set { _cf = value; }
         }

         public RRA.CF reduce
         {
             get { return _reduce; }
             set { _reduce = value; }
         }
        
         public override string ToString()
         {
             string _step_  = (step > 0)             ? ":step="   + step    : "";
             string _start_ = (start != "")          ? ":start="  + start   : "";
             string _stop_  = (stop != "")           ? ":end="   + stop    : "";
             string _reduce_ = (reduce != RRA.CF.NA) ? ":reduce=" + reduce  : "";

             return "DEF:" + name + "=" + Path.GetFileName(_rrd.filename) + ":" + ds + ":" + cf.ToString()
                           + _step_ + _start_ + _stop_ + _reduce_;
         }


         private bool check_format(string val)
         {
             if (val.Contains(" "))
                 return false;

             return true;
         }


         private void RemovePadding(String[] vals)
         {
             for (int i = 0; i < vals.Length; i++)
                 vals[i] = vals[i].Trim();
         }

#if TEST_GRAPH_DEF
         public static void Main()
         {
             NHawkCommand.Instance.RRDCommandPath = @"E:\downloads\rrdtool\Release\rrdtool.exe";
             DEF def = null;
            
             Console.WriteLine("Testing the DEF class promotion ctor #1 (used for graphing))");
            
             RRD rrd = new RRD("test.rrd", 920805000, 300,
                              (new DS("ds1", DS.TYPE.ABSOLUTE, 600, DS.U, DS.U)),
                              (new RRA(RRA.CF.AVERAGE, .5, 12, 24)));
             rrd.create(true);

             try
             {
                 Console.WriteLine("Note: this should throw because there is no DS \"somedef\" defined in rrd1");
                 def = new DEF("def1", rrd, "somedef", RRA.CF.AVERAGE);
             }
             catch (NHAWK.DataSourceException ex)
             {
                 Console.WriteLine("In exception handler: {0}", ex.Message);
             }

             try
             {
                 Console.WriteLine("Note: this should throw because there is no RRA \"LAST\" defined in rrd1");
                 def = new DEF("def1", rrd, rrd.ds_lookup(0).name, RRA.CF.LAST);
             }
             catch (NHAWK.RRACFException ex)
             {
                 Console.WriteLine("In exception handler: {0}", ex.Message);
             }


             try
             {
                 Console.WriteLine("Note: this should throw because bad definition name format");
                 def = new DEF("de f1", rrd, rrd.ds_lookup(0).name, RRA.CF.AVERAGE);
             }
             catch (NHAWK.DefException ex)
             {
                 Console.WriteLine("In exception handler: {0}", ex.Message);
             }
            
             
             //                         "ds1"                  "AVERAGE" 
             def = new DEF("def1", rrd, rrd.ds_lookup(0).name, rrd.rra_lookup(0).consolidationFunc);

             Console.WriteLine("DEF: def1");
             Console.WriteLine("Hashcode     : {0}", def.GetHashCode());
             Console.WriteLine("Name         : {0}", def.name);
             Console.WriteLine("CF           : {0}", def.cf);
             Console.WriteLine("DS           : {0}", def.ds);
             Console.WriteLine("Serialization: {0}", def.ToString());

             Console.WriteLine("\nTesting protomotion ctor # 2");
             DEF def2 = new DEF("def_name", "test.rrd", "ds1", RRA.CF.AVERAGE);

             Console.WriteLine("DEF: def2");
             Console.WriteLine("Hashcode     : {0}", def2.GetHashCode());
             Console.WriteLine("Name         : {0}", def2.name);
             Console.WriteLine("CF           : {0}", def2.cf);
             Console.WriteLine("DS           : {0}", def2.ds);
             Console.WriteLine("Step         : {0}", def2.step);
             Console.WriteLine("Start        : {0}", def2.start);
             Console.WriteLine("Stop         : {0}", def2.stop);
             Console.WriteLine("Reduce       : {0}", def2.reduce);
             Console.WriteLine("Serialization: {0}", def2.ToString());

             Console.WriteLine("\nTesting protomotion ctor # 3");
             DEF def3 = new DEF("def_name", "test.rrd", "ds1", RRA.CF.AVERAGE, 7200);

             Console.WriteLine("DEF: def3");
             Console.WriteLine("Hashcode     : {0}", def3.GetHashCode());
             Console.WriteLine("Name         : {0}", def3.name);
             Console.WriteLine("CF           : {0}", def3.cf);
             Console.WriteLine("DS           : {0}", def3.ds);
             Console.WriteLine("Step         : {0}", def3.step);
             Console.WriteLine("Serialization: {0}", def3.ToString());
             

             Console.WriteLine("\nTesting protomotion ctor # 4");
             DEF def4 = new DEF("def_name", "test.rrd", "ds1", RRA.CF.AVERAGE, 7200, "start+1h", "end-1h", RRA.CF.AVERAGE);

             Console.WriteLine("DEF: def4");
             Console.WriteLine("Hashcode     : {0}", def4.GetHashCode());
             Console.WriteLine("Name         : {0}", def4.name);
             Console.WriteLine("CF           : {0}", def4.cf);
             Console.WriteLine("DS           : {0}", def4.ds);
             Console.WriteLine("Step         : {0}", def4.step);
             Console.WriteLine("Start        : {0}", def4.start);
             Console.WriteLine("Stop         : {0}", def4.stop);
             Console.WriteLine("Reduce       : {0}", def4.reduce);
             Console.WriteLine("Serialization: {0}", def4.ToString());

             Console.WriteLine("\nTesting protomotion ctor # 5");
             DEF def5 = new DEF("def_name", RRD.load("test.rrd"), "ds1", RRA.CF.AVERAGE, 7200, "11\\:00", "end-1h", RRA.CF.AVERAGE);

             Console.WriteLine("DEF: def5");
             Console.WriteLine("Hashcode     : {0}", def5.GetHashCode());
             Console.WriteLine("Name         : {0}", def5.name);
             Console.WriteLine("CF           : {0}", def5.cf);
             Console.WriteLine("DS           : {0}", def5.ds);
             Console.WriteLine("Step         : {0}", def5.step);
             Console.WriteLine("Start        : {0}", def5.start);
             Console.WriteLine("Stop         : {0}", def5.stop);
             Console.WriteLine("Reduce       : {0}", def5.reduce);
             Console.WriteLine("Serialization: {0}", def5.ToString());

             Console.WriteLine("\nTesting DeepClone: (notice hash codes for def5 and def6");
             DEF def6 = DeepClone(def5);

             Console.WriteLine("DEF: def6");
             Console.WriteLine("Hashcode     : {0}", def6.GetHashCode());
             Console.WriteLine("Name         : {0}", def6.name);
             Console.WriteLine("CF           : {0}", def6.cf);
             Console.WriteLine("DS           : {0}", def6.ds);
             Console.WriteLine("Start        : {0}", def6.start);
             Console.WriteLine("Stop         : {0}", def6.stop);
             Console.WriteLine("Step         : {0}", def6.step);
             Console.WriteLine("Reduce       : {0}", def6.reduce);
             Console.WriteLine("Serialization: {0}", def6.ToString());


             Console.WriteLine("\nTest deserialization constructor");
             DEF def7 = new DEF(def6.ToString());
             Console.WriteLine("DEF: def7");
             Console.WriteLine("Hashcode     : {0}", def7.GetHashCode());
             Console.WriteLine("Name         : {0}", def7.name);
             Console.WriteLine("CF           : {0}", def7.cf);
             Console.WriteLine("DS           : {0}", def7.ds);
             Console.WriteLine("Start        : {0}", def7.start);
             Console.WriteLine("Stop         : {0}", def7.stop);
             Console.WriteLine("Step         : {0}", def7.step);
             Console.WriteLine("Reduce       : {0}", def7.reduce);
             Console.WriteLine("Serialization: {0}", def7.ToString());
         }
#endif 
     }
}