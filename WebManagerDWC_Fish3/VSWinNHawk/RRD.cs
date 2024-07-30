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
//File       : RRD.cs   -- provides .Net / Mono wrapper around the RRDTool Round Robin Database (RRD)    //
//Application: NHawk: Open Source Project Support                                                        // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                           //
//Author     : Mike Corley, Syracuse University                                                          //
//             mwcorley@syr.edu                                                                          //
///////////////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides a C# abstraction which provides a .Net interface to the RRDTool RRD (Round Robin Database) 
 * construct.  This class seeks to encapsulate the RRD in C# .Net object model, while retaining intuitive use of 
 * the RRDTool syntax. It also seeks to enforce code correctness enforcing proper use of constructs by perserving 
 * RRDTool level relationships and dependenices between corresponding entities.  This is provided with appropriate 
 * exception handling.  This class was developed with intention of perserving the RRDTool syntax and semantics, 
 * enabling .Net developers to rapidy gain development productivity refering directly to the Tobi Oetiker's 
 * main RRDTool documentation page: http://oss.oetiker.ch/rrdtool/doc/rrdcreate.en.html
 * 
 * Public Interface
 * ================
 * public RRD(string filename, long start)
 *   -- constructor: constructs a basic RRD (accepts filename and starting timestamp in UTC)
 *      (note: you must define at least one Data Source and one Round Robin Archive using addDS() 
 *      and addRAA() and RRDFormatException will be thrown on create() and ToString() operations)
 *       
 * public RRD(string filename, long start, long step)
 *   -- constructor: *** same as previous constructor, this ctor also allows specification of the step
 *   
 * public RRD(string filename, long start, long step, DS ds, RRA rra)
 *   -- constructor: constructs a fully defined RRD with specification for default DS and RRA
 *      (note: improper RRD, DS and RRA definitions will cause the appropriate exception to be thrown:
 *             RRDFormatException, DSFormatException, RRAFormatException)
 *      
 * public RRD(string filename, long start,  long step, List<DS> ds_list, List<RRA> rra_list)
 *   -- constructor: constructs a fully defined RRD with specification for a complete list 
 *      of data sources (DS) and round robin archives (rras)
 *      (note: improper RRD, DS and RRA definitions will cause the appropriate exception to be thrown:
 *             RRDFormatException, DSFormatException, RRAFormatException)
 *      
 * public RRD(string rrd)
 *   -- construct an RRD through deserialization of an existing RRD string
 *      if the format is incorrect the exception to be thrown: RRDFormatException, DSFormatException, RRAFormatException
 *   
 * public void create(bool overwrite)
 *   -- writes the RRD out file/path specified by the "filename" property
 *      if the file exists and overwrite== false, and RRDFileExistsException
 *      is thrown, otherwise the file is either created or overwritten
 *      
 * public void update(long ts, object[] values)
 *   -- updates the RRD at time "ts" with arguments for corresponding data sources packed in
 *      array "values".  Arguments bust be passed in the same order in which data sources appear in 
 *      RRD definition.  The wrong number of arguments passed will result with an: RRDUpdateException 
 *      being thrown.  if the filename (specifed by property "filename") cannot be found: a RRDFileNotFoundException
 *      exception will be thrown.
 *      
 * public static RRD Load(string rrd_path);
 *   -- opens an existing RRD for updating and graphing from the path specified by: rrd_path
 *      on error, the appropriate exception is thrown:  RRDFileNotFoundException RRDFormatException, 
 *                                                      DSFormatException, RRAFormatException
 * public static RRD DeepClone(RRD rrd)
 *   -- returns a deep copy of rrd
 *    
 * public DS ds_lookup(string ds_name)
 *   -- returns a reference to the data source specified by ds_name, or null if it doesn't exist
 *   
 * public DS ds_lookup(int index)
 *   -- returns a reference to the data source specified by index number (0 based) or null if it doesn't exists
 *  
 * public RRA rra_lookup(int index)
 *    -- returns a reference to the rra specified by index number (0 based) or null if it doesn't exists
 *   
 * public void addDS(DS ds)
 *    -- adds a new DS reference to DS list.  
 *       an attempt to add a DS with the same name (duplicate) as a previously
 *       added DS with cause a DSFormatException to be thrown
 *       improper DS definition will cause an DSFormatException to be thrown
 *       
 * public void addRRA(RRA rra)
 *    -- adds a new RRA reference to the RRA list.
 *       improper RRA definition will cause an: RRAFormatException to be thrown
 * 
 * public override string ToString()
 *     -- serializing the instance state to a string
 *     
 * public void export(string path, bool append)
 *    -- exports the RRD definition to the path: path, if append = true, the RRD definition is eppended to an existing
 *       file, if aleady exists, otherwise a new file is created.  This provides a facility for working directly to and
 *       from the RRDTool executable and/or existing RRDTool scripts.
 *       
 * public long start
 *    -- gets or sets the RRD starting time specifier
 *    
 * public long step
 *    -- gets or sets the step (sample rate in seconds)
 *        attempt to set < 0 will cause an RRDFormatException to be thrown
 *    
 * public string filename
 *    -- gets or sets the path (filename) for underlying RRD file
 *    
 * public int ds_count
 *    -- gets the number of data sources currently added to the RRD def. instance
 * 
 * public int rra_Count
 *    -- gets the number of round robin archives currently added to the RRD def. instance
 *    
 * public static long getTS(DateTime dt)
 *    -- return unix sytle (UTC) timestamp representation for the DateTime object passed (dt)
 *    
 * public static long tsNow
 *    -- return unix sytle (UTC) timestamp representation for the for the current system time
 *     
 * public static long origin
 *    -- returns 0 (unix timestamp (UTC) for elpased seconds since January 01, 1970 12:00AM)
 *     
 * public static DateTime StrTime(long ts)
 *    --  returns DateTime object for the unix timestamp passed in (ts)
 *    
 * 
 *  Example Usage
 *  =============
 *  See Test stub below
 * 
 *  Build Process
 *  =============  
 *  This module can be compiled stand alone for testing purposes: 
 *  Note: some modules have additional assemblies dependencies which
 *     are needed for compilation of the test stub.  The required files section
 *     shows true dependency structure. i.e when the test stub is not compiled.
 *
 *  Compiler Command:
 *  Visual Studio 2008 (.Net 3.5): csc  /define:TEST_RRD  RRD.cs ..\DS\DS.cs ..\RRA\RRA.cs  ..\NHawkCommand\NHawkCommand.cs
 *  Win32 Mono (1.9.1)           : gmcs -define:TEST_RRD  RRD.cs ..\DS\DS.cs ..\RRA\RRA.cs  ..\NHawkCommand\NHawkCommand.cs
 *  SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_RRD  RRD.cs ../DS/DS.cs ../RRA/RRA.cs  ../NHawkCommand/NHawkCommand.cs
 *                                                                  
 *  Required Files:   RRD.cs DS.cs RRA.cs NHawkCommand.cs
 * 
 *  Maintanence History
 *  ===================
 *  version 1.0 : 01 August 08 
 *     -- first release: 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace NHAWK
{
    [Serializable]
    public class RRDFileNotFoundException : Exception
    {
        public RRDFileNotFoundException(string message)
            : base(message)
        { }

        public RRDFileNotFoundException(string message, Exception ex)
            : base(message, ex)
        { }
    };

    [Serializable]
    public class RRDFileExistsException : Exception
    {
        public RRDFileExistsException(string message)
            : base(message)
        { }

        public RRDFileExistsException(string message, Exception ex)
            : base(message, ex)
        { }
    };
   
    [Serializable]
    public class RRDFormatException : Exception
    {
        public RRDFormatException(string message)
            : base(message)
        { }

        public RRDFormatException(string message, Exception ex)
            : base(message, ex)
        { }
    };

    [Serializable]
    public class RRDUpdateException : Exception
    {
        public RRDUpdateException(string message)
            : base(message)
        { }

        public RRDUpdateException(string message, Exception ex)
            : base(message, ex)
        { }
    };

 
    [Serializable]
    public class RRD 
    {
        List<RRA>     _rra_list;
        List<DS>      _ds_list;
        string        _filename;
        string        _current_update_str;
        string        _working_dir_;
        long          _step;
        long          _start;
        const int     DEFAULT_STEP = 300;


        public RRD(string filename, long start)
        {
            _rra_list = new List<RRA>();
            _ds_list = new List<DS>();
            _current_update_str = "";
            _working_dir_ = Directory.GetCurrentDirectory();
            this.filename = filename;
            this.start = start;
            this.step = DEFAULT_STEP;
        }


        public RRD(string filename, long start, long step): this(filename, start)
        {
            this.step = step;
        }


        public RRD(string filename, long start, long step, DS ds, RRA rra): this(filename, start, step)
        {
            addDS(ds);
            addRRA(rra);
        }


        public RRD(string filename, long start,  long step, List<DS> ds_list, List<RRA> rra_list): this(filename, start, step)
        {
            _ds_list = ds_list;
            _rra_list = rra_list;
        }


        public RRD(string rrd)
        {
            string[] first_level_tokens;
            try
            {
                Init(this);

                this._working_dir_ = Directory.GetCurrentDirectory();
                first_level_tokens = rrd.Split(new char[] { ' ', '\t' });
                RemoveSlashes(first_level_tokens);

                if(first_level_tokens[0].Trim().ToLower() != "create")
                   throw new RRDFormatException("Bad syntax: expected \"create\" command: " + first_level_tokens[0].Trim().ToLower());
                else
                   filename = first_level_tokens[1].Trim();

                _ds_list = new List<DS>(); _rra_list = new List<RRA>();
                for (int i = 2; i < first_level_tokens.Length; i++)
                {
                    switch (first_level_tokens[i].Trim())
                    {
                        case "--start":
                        case "-b":
                            start = long.Parse(first_level_tokens[++i].Trim());
                            continue;
                      
                        case "--step":
                        case "-s":
                            step = long.Parse(first_level_tokens[++i].Trim());
                            continue;    
                    };

                    //parse as potential data source
                    if (first_level_tokens[i].Trim().ToUpper().IndexOf("DS") == 0)
                    {
                        _ds_list.Add(new DS(first_level_tokens[i].Trim()));
                        continue;
                    }

                    //parse as potential RRA
                    if (first_level_tokens[i].Trim().ToUpper().IndexOf("RRA") == 0)
                    {
                        _rra_list.Add(new RRA(first_level_tokens[i].Trim()));
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is RRAFormatException || ex is DSFormatException || ex is RRDFormatException)
                    throw;
                else
                   throw new RRDFormatException("Bad RRDtool \"create\" commmand format", ex);
            }
        }

       
        public string workingDir
        {
            get { return _working_dir_; }
            set { _working_dir_ = value; }
        }

        public void create(bool overwrite)
        {
            if (File.Exists(filename))
                if (!overwrite)
                    throw new RRDFileExistsException("RRD " + filename + " already exists... use overwite=true");

            NHawkCommand.Instance.RunCommand(ToString());
        }

        public bool Exists
        {
            get
            {
                if (File.Exists(filename))
                    return true;
                else
                    return false;
            }
        }


      
        public void update(long ts, string[] values)
        {
            if (!Exists)
                throw new RRDFileNotFoundException(filename + " could not be found! Did you change the filename??  If so, see doc. on DeepClone()");

            /* this needs some work
            if(Start >= ts)
                throw new RRDUpdateException(Filename + "must be later than" + Start.ToString() + " got " + ts.ToString() );
            */

            //if (DS_Count == 0 || RRA_Count == 0)
            //    throw new DSFormatException("An RRD must define at least one DS and at least one RRA");


            if (values.Length != this.ds_count)
                throw new RRDUpdateException(filename + " expects " + rra_count.ToString() + " arguments got " + values.Length.ToString());

            StringBuilder update_builder = new StringBuilder();
            foreach (string value in values)
                update_builder.Append(":" + value);

            _current_update_str = ("update " + filename + " " + ts.ToString() + update_builder.ToString());

            NHawkCommand.Instance.RunCommand(CurrentUpdateStr);
        }


        public void update(long ts, object[] values)
        {
            if (!Exists)
                throw new RRDFileNotFoundException(filename + " could not be found! Did you change the filename??  If so, see doc. on DeepClone()");

            /* this needs some work
            if(Start >= ts)
                throw new RRDUpdateException(Filename + "must be later than" + Start.ToString() + " got " + ts.ToString() );
            */

            //if (DS_Count == 0 || RRA_Count == 0)
            //    throw new DSFormatException("An RRD must define at least one DS and at least one RRA");


            if (values.Length != this.ds_count)
                throw new RRDUpdateException(filename + " expects " + rra_count.ToString() + " arguments got " + values.Length.ToString());

            StringBuilder update_builder = new StringBuilder();
            foreach (object value in values)
                update_builder.Append(":" + value);

            _current_update_str = ("update " + filename + " " + ts.ToString() + update_builder.ToString());
     
            NHawkCommand.Instance.RunCommand(CurrentUpdateStr);
            
        }

        public static RRD DeepClone(RRD rrd)
        {
            List<DS> new_ds_list = new List<DS>();
            List<RRA> new_rra_list = new List<RRA>();

            foreach (DS ds in rrd._ds_list)
                new_ds_list.Add(DS.DeepClone(ds));

            foreach (RRA rra in rrd._rra_list)
                new_rra_list.Add(RRA.DeepClone(rra));

            return new RRD((new StringBuilder().Append(rrd._filename)).ToString(), rrd.start, rrd.step, new_ds_list, new_rra_list);
        }


        public static RRD load(string rrd_filename)
        {
            //rrdtool dump filename.rrd >filename.xml
           if (!File.Exists(rrd_filename))
                throw new RRDFileNotFoundException("The RRD File: " + rrd_filename + " could not be found");
           
            RRD rrd = null;
            Stream data_stream = null;
            string version = "not determined";
            try
            {
                data_stream = GetRRDFileAsStream(rrd_filename);
                XmlDocument rrd_doc = new XmlDocument();
                rrd_doc.Load(data_stream);
       
                version = rrd_doc.DocumentElement.ChildNodes[0].InnerText.Trim();
                rrd = new RRD(rrd_filename, GetStartTimeFromRRD(rrd_filename), long.Parse(rrd_doc.DocumentElement.ChildNodes[1].InnerText.Trim()));

                //parse out data source list
                XmlNodeList ds_list = rrd_doc.GetElementsByTagName("ds");
                foreach (XmlNode ds_node in ds_list)
                    if ((ds_node.ChildNodes.Count == 9) && (ds_node.HasChildNodes) && (ds_node.FirstChild.Name == "name"))
                    {
                        string min = ds_node.ChildNodes[3].InnerText.Trim();
                        string max = ds_node.ChildNodes[4].InnerText.Trim();
                        rrd.addDS(new DS(ds_node.ChildNodes[0].InnerText.Trim(),
                                         DS.StringToType(ds_node.ChildNodes[1].InnerText.Trim()),
                                         long.Parse(ds_node.ChildNodes[2].InnerText.Trim()),
                                         Double.Parse(min),
                                         Double.Parse(max)));
                    }

                //parse of the RRA list
                XmlNodeList rra_list = rrd_doc.GetElementsByTagName("rra");
                foreach (XmlNode rra in rra_list)
                    // if (rra.ChildNodes.Count == 5)
                    rrd.addRRA(new RRA(RRA.StringToCF(rra.ChildNodes[0].InnerText.Trim()),
                                       Double.Parse(rra.ChildNodes[3].ChildNodes[0].InnerText.Trim()),
                                       int.Parse(rra.ChildNodes[1].InnerText.Trim()),
                                       int.Parse(rra.ChildNodes[5].ChildNodes.Count.ToString())));
            }
            catch (Exception ex)
            {
                if (ex is RRAFormatException || ex is DSFormatException || ex is RRDFormatException)
                    throw;
                else
                    throw new RRDFormatException("Could not load RRD file: " + rrd_filename + "version: " + version, ex);
            }
            finally
            {
                if (data_stream != null)
                {
                    data_stream.Close();
                    data_stream.Dispose();
                }
            }

           return rrd;
        }

        public string CurrentUpdateStr
        {
            get { return _current_update_str; }
        }

        public long start
        {
            get { return _start; }
            set
            {
                if (value <= origin)
                    throw new RRDFormatException("RRD cannot begin before: " + StrTime(origin));

                _start = value;
            }
        }

        public long step
        {
            get { return _step; }
            set
            {
                if (value < 0)
                    throw new RRDFormatException("The step value cannot be negative");

                _step = value;
            }
        }

        public string filename
        {
            get { return _filename; }
            set
            {
                    _filename = value.Trim();

                if (Path.GetDirectoryName(value.Trim()) != "")
                    _working_dir_ = Path.GetDirectoryName(value.Trim());
            }
        }

        public int ds_count
        {
            get
            {
                if (_ds_list != null)
                    return _ds_list.Count;

                return 0;
            }
        }

        public int rra_count
        {
            get
            {
                if (_rra_list != null)
                    return _rra_list.Count;

                return 0;
            }
        }

        public DS ds_lookup(string ds_name)
        {
            foreach(DS ds in _ds_list)
                if (ds.name == ds_name)
                    return ds;

            return null;
        }

        public DS ds_lookup(int index)
        {
            if (index >= 0 && index < ds_count)
                return _ds_list[index];
               
            return null;
        }

        public RRA rra_lookup(int index)
        {
            if (index >= 0 && index < rra_count)
                return _rra_list[index];
           
            return null;
        }
    
        public void addDS(DS ds)
        {
            if (!ContainsDS(ds.name))
                _ds_list.Add(ds);
            else
                throw new RRDFormatException(filename + " already contains a DS " + ds.name);
        }

        public void addRRA(RRA rra)
        {
              _rra_list.Add(rra);
        }

        public override string ToString()
        {
            if ((_ds_list.Count == 0) || (_rra_list.Count == 0))
                throw new RRDFormatException("RRD definition must have at least one DS and at least one RRA");
  
            StringBuilder ds_list_builder = new StringBuilder();
            foreach (DS ds in _ds_list)
                ds_list_builder.Append(ds.ToString() + " ");
        
            StringBuilder rra_list_builder = new StringBuilder();
            foreach (RRA rra in _rra_list)
                rra_list_builder.Append(rra.ToString() + " ");
           
            return "create " + filename + " --start " + start + " --step " + step + " " + ds_list_builder.ToString() + rra_list_builder.ToString(); 
        }


        public void export(string path, bool append)
        {
            StreamWriter sw = null; 
            try
            {
              sw = new StreamWriter(path, append);
              sw.Write(ToString());
              sw.Close();
              sw.Dispose();
            }
            finally
            {
               if(sw != null)
               {
                  sw.Close();
                  sw.Dispose();
               }
            }
        }

        public static long getTS(DateTime dt)
        {
            return (long)dt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }


        public static long tsNow
        {
            get { return getTS(DateTime.Now); }
        }


        public static long origin
        {
            get
            {
                return 0;
            }
        }


        public static DateTime StrTime(long ts)
        {
           return (new DateTime(1970,1,1)).AddSeconds((double)ts);     
        }
        

        private bool ContainsDS(string name)
        {
            foreach (DS ds in _ds_list)
                if (ds.name == name)
                    return true;
            return false;
        }


        private void RemoveSlashes(String[] slashes)
        {
            for (int i = 0; i < slashes.Length; i++)
                slashes[i] = slashes[i].Replace("\\", "");
        }


        private static Stream GetRRDFileAsStream(string rrd_filename)
        {
            string command = "dump " + rrd_filename;
            Stream memstream = NHawkCommand.Instance.RunCommandRedirect(command);
            return memstream;
        }


        private static long GetStartTimeFromRRD(string rrd_filename)
        {
            string command = "first " + rrd_filename;
            Stream memstream = NHawkCommand.Instance.RunCommandRedirect(command);

            Byte[] buf = new Byte[memstream.Length];
            memstream.Read(buf, 0, (int)memstream.Length);
            ASCIIEncoding enc = new ASCIIEncoding();
            string str_ts = enc.GetString(buf);

            long ts = long.Parse(str_ts);
            memstream.Close();
            memstream.Dispose();

            return ts;
        }

        private static void Init(RRD rrd)
        {
            rrd._rra_list = new List<RRA>();
            rrd._ds_list = new List<DS>();
            rrd._current_update_str = "";
            rrd._working_dir_ = Directory.GetCurrentDirectory();
            rrd.filename = "na";
            rrd.start = tsNow;
            rrd.step = DEFAULT_STEP;
        }


#if TEST_RRD
        static void Main(string[] args)
        {

            NHawkCommand.Instance.RRDCommandPath = @"e:\rrdtool\release\rrdtool.exe";

            Console.WriteLine("Testing RRD class operation");
            Console.WriteLine("===========================");

            Console.WriteLine("Testing Promotion ctor #1 for RRD1");
            RRD rrd1 = new RRD("test.rrd", RRD.tsNow);
            rrd1.addDS(new DS("bandwidth", DS.TYPE.COUNTER, 600, 0, DS.U));
            rrd1.addDS(new DS("bandwidth2", DS.TYPE.GAUGE, 900, -1.2, 74.3));
            rrd1.addRRA(new RRA(RRA.CF.AVERAGE, 0.5, 1, 24));
            rrd1.addRRA(new RRA(RRA.CF.AVERAGE, 0.5, 5, 600));
            rrd1.create(true);

            Console.WriteLine("Testing Insertion into RRD1");
            int i;
            for (i = 0; i < 20; i++)
            {
                long simulated_timestamp = (RRD.tsNow) + ((i + 1) * 300);
                rrd1.update(simulated_timestamp, new object[] { (i + 1000), (i + 2000) });
                Console.WriteLine("Update Str: {0}", rrd1.CurrentUpdateStr);
            }
            Console.WriteLine("Successfully inserted: {0} values into rrd1", i);

            try
            {
                Console.WriteLine("Testing RRDUpdateException... should throw due to wrong number of arguments");
                for (i = 0; i < 20; i++)
                {
                    long simulated_timestamp = 920804700 + ((i + 1) * 300);
                    rrd1.update(simulated_timestamp, new object[] { (i + 2000) });
                }
            }
            catch (RRDUpdateException ex)
            {
                Console.WriteLine("In exception handler: " + ex.Message);
            }

            Console.WriteLine("\nTest \"FileExists\" exception propagation... should throw because overwrite = false");
            try
            {
                rrd1.create(false);
            }
            catch (RRDFileExistsException ex)
            {
                Console.WriteLine("In Exception handler {0}", ex.Message);
            }

            Console.WriteLine("Hash code: {0}", rrd1.GetHashCode());
            Console.WriteLine("Filename : {0}", rrd1.filename);
            Console.WriteLine("Start    : {0}", rrd1.start);
            Console.WriteLine("Step     : {0}", rrd1.step);
            Console.WriteLine("DS Count : {0}", rrd1.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd1.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd1.ToString());

            Console.WriteLine("\nTest load of existing RRD File");
            rrd1 = RRD.load("test.rrd");

            Console.WriteLine("Hash code: {0}", rrd1.GetHashCode());
            Console.WriteLine("Filename : {0}", rrd1.filename);
            Console.WriteLine("Start    : {0}", rrd1.start);
            Console.WriteLine("Step     : {0}", rrd1.step);
            Console.WriteLine("DS Count : {0}", rrd1.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd1.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd1.ToString());

            Console.WriteLine("\nTest \"FileNotFound\" exception propagation for RRD.Load()");
            try
            {
                rrd1 = RRD.load("test12345432.rrd");
            }
            catch (RRDFileNotFoundException ex)
            {
                Console.WriteLine("In exception handler: {0}", ex.Message);
            }

            Console.WriteLine("\nTesting Promotion ctor #2 for RRD2");
            RRD rrd2 = new RRD("myspeed.rrd", 920804700, 120);
            rrd2.addDS(new DS("speed", DS.TYPE.COUNTER, 240, 0, 100));
            rrd2.addRRA(new RRA(RRA.CF.AVERAGE, 0.5, 30, 12));

            Console.WriteLine("Hash code: {0}", rrd2.GetHashCode());
            Console.WriteLine("Filename : {0}", rrd2.filename);
            Console.WriteLine("Start    : {0}", rrd2.start);
            Console.WriteLine("Step     : {0}", rrd2.step);
            Console.WriteLine("DS Count : {0}", rrd2.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd2.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd2.ToString());

            Console.WriteLine("\nTesting Promotion ctor #3 for RRD3");
            RRD rrd3 = new RRD("airspeed.rrd", 920804700, 10,
                       new DS("speed", DS.TYPE.COUNTER, 20, 0, 600),
                       new RRA(RRA.CF.AVERAGE, 0.5, 6, 60));

            Console.WriteLine("Testing ADD singular DS and RRA");
            rrd3.addDS(new DS("f16_speed", DS.TYPE.COUNTER, 2, 0, 2000));
            rrd3.addRRA(new RRA(RRA.CF.MAX, 0.5, 6, 60));

            Console.WriteLine("Hash code: {0}", rrd3.GetHashCode());
            Console.WriteLine("Filename : {0}", rrd3.filename);
            Console.WriteLine("Start    : {0}", rrd3.start);
            Console.WriteLine("Step     : {0}", rrd3.step);
            Console.WriteLine("DS Count : {0}", rrd3.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd3.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd3.ToString());


            Console.WriteLine("\nTesting Promotion ctor #4 for RRD4");
            Console.WriteLine("Testing DS_Lookup()  and DS  serialization (ToString()) members");
            Console.WriteLine("Testing RRA_Lookup() and RAA serialization  (ToString() members");

            List<DS> ds_list = new List<DS>();
            List<RRA> rra_list = new List<RRA>();

            ds_list.Add(new DS(rrd3.ds_lookup("f16_speed").ToString()));
            ds_list.Add(new DS(rrd3.ds_lookup(0).ToString()));
            rra_list.Add(new RRA(rrd3.rra_lookup(0).ToString()));

            RRD rrd4 = new RRD("test1234.rrd", 920804700, 300, ds_list, rra_list);

            Console.WriteLine("Hash code: {0}", rrd4.GetHashCode());
            Console.WriteLine("Filename : {0}", rrd4.filename);
            Console.WriteLine("Start    : {0}", rrd4.start);
            Console.WriteLine("Step     : {0}", rrd4.step);
            Console.WriteLine("DS Count : {0}", rrd4.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd4.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd4.ToString());

            Console.WriteLine("\nTesting RRD Deserialization:");
            RRD rrd5 = new RRD(rrd4.ToString());
            Console.WriteLine("Hash code: {0}", rrd5.GetHashCode());
            Console.WriteLine("Filename : {0}", rrd5.filename);
            Console.WriteLine("Start    : {0}", rrd5.start);
            Console.WriteLine("Step     : {0}", rrd5.step);

            Console.WriteLine("DS Count : {0}", rrd5.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd5.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd5.ToString());

            Console.WriteLine("\nTesting RRD deep cloning for RRD6:  notice hashcodes for rrd5 and rrd6 ");
            RRD rrd6 = RRD.DeepClone(rrd5);
            Console.WriteLine("Hash code: {0}", rrd6.GetHashCode());
            Console.WriteLine("Filename : {0}", rrd6.filename);
            Console.WriteLine("Start    : {0}", rrd6.start);
            Console.WriteLine("Step     : {0}", rrd6.step);
            Console.WriteLine("DS Count : {0}", rrd6.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd6.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd6.ToString());

            Console.WriteLine("Testing RRD class mutators rrd6 ");
            rrd6.filename = "new_filename.rrd";
            rrd6.start = 920805000;
            rrd6.step = 900;
            Console.WriteLine("Filename : {0}", rrd6.filename);
            Console.WriteLine("Start    : {0}", rrd6.start);
            Console.WriteLine("Step     : {0}", rrd6.step);
            Console.WriteLine("DS Count : {0}", rrd6.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd6.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd6.ToString());

            Console.WriteLine("\nTesting Export function");
            rrd6.export("test.txt", false);

            Console.WriteLine("Testing Import through serialization: rrd7");
            StreamReader str = new StreamReader("test.txt");
            RRD rrd7 = new RRD(str.ReadToEnd());
            str.Close();
            str.Dispose();

            Console.WriteLine("Hash code: {0}", rrd7.GetHashCode());
            Console.WriteLine("Filename : {0}", rrd7.filename);
            Console.WriteLine("Start    : {0}", rrd7.start);
            Console.WriteLine("Step     : {0}", rrd7.step);
            Console.WriteLine("DS Count : {0}", rrd7.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd7.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd7.ToString());


            Console.WriteLine("\n\n*** Finally, Testing file access in remote directories");
            RRD rrd_remote = new RRD("e:\\test_remote.rrd", 920804700);
            rrd_remote.addDS(new DS("bandwidth", DS.TYPE.COUNTER, 600, 0, DS.U));
            rrd_remote.addDS(new DS("bandwidth2", DS.TYPE.GAUGE, 900, -1.2, 74.3));
            rrd_remote.addRRA(new RRA(RRA.CF.AVERAGE, 0.5, 1, 24));
            rrd_remote.addRRA(new RRA(RRA.CF.AVERAGE, 0.5, 5, 600));
            rrd_remote.create(true);


            Console.WriteLine("Filename : {0}", rrd_remote.filename);
            Console.WriteLine("Start    : {0}", rrd_remote.start);
            Console.WriteLine("Step     : {0}", rrd_remote.step);
            Console.WriteLine("DS Count : {0}", rrd_remote.ds_count);
            Console.WriteLine("RRA Count: {0}", rrd_remote.rra_count);
            Console.WriteLine("Testing RRD Serialization: {0}", rrd_remote.ToString());

            for (i = 0; i < 20; i++)
            {
                long simulated_timestamp = 920804700 + ((i + 1) * 300);
                rrd_remote.update(simulated_timestamp, new object[] { (i + 1000), (i + 2000) });
                Console.WriteLine("Update Str: {0}", rrd_remote.CurrentUpdateStr);
            }
        }
#endif
    }

}
