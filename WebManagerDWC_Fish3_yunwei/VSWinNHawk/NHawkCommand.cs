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
//File       : NHawkCommand.cs   -- provides .Net / Mono interface to RRDTool executable                 //
//Application: NHawk: Open Source Project Support                                                        // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                           //
//Author     : Mike Corley, Syracuse University                                                          //
//             mwcorley@syr.edu                                                                          //
///////////////////////////////////////////////////////////////////////////////////////////////////////////

/*Module Operations
 * ================
 * This class provides a singleton interface for invoking the RRDTool executable. Currently the only NHawk 
 * constructs that use this class are the RRD class for creating and updating round robin databases, and the
 * GRAPH class for creating the visualizings.  This works by respective Nawk object instance serializing itself, 
 * then passing the serialized form to RRDtool to be carried out. 
 * 
 * Public Interface
 * ================
 *
 * public class RRDCommandException : Exception
 * public class NHawkCommand
 * 
 * public string RRDCommandPath
 * -- gets or sets the path of the RRDtool executable
 *    Note: if RRDtool is not in the path statement, then this 
 *    method must be invoked with the RRDtool executable path, 
 *    otherwise a RRDCommandException will be thrown.
 *    
 * public void RunCommand(string arguments)
 * -- invokes the the RRdtool execuable with "arguments" passed in
 *    if the RRDtool executable writes to stderr for any reason, then stderr,
 *    is redirected and returned through an "RRDCommandException" 
 * 
 * public Stream RunCommandRedirect(string command)
 * -- executes the command passed in as "command", and redirects, captures and
 *    returns stdout as Stream: current used only by RRD.load(..)
 *    
 * Example Usage
 * =============
 * NHawkCommand.Instance.RRDCommandPath = @"e:\downloads\rrdtool\release\rrdtool.exe";
 *   -- for win32
 * NHawkCommand.Instance.RRDCommandPath = "/usr/bin/rrdtool"
 *   -- Linux
 *   
 * Build Process
 * =============
 * Compiler Command:
 *   Visual Studio 2008  : csc /define:TEST_NHawkCommand  NHawkCommand.cs
 *   Win32 Mono 1.9.1    : gmcs -define:TEST_NHawkCommand NHawkCommand.cs
 *   Suse 10.3 Mono 1.9.1: gmcs -define:TEST_NHawkCommand NHawkCommand.cs
 *   
 * Maintanence History
 * ===================
 * version 1.0 : 01 August 08 
 *    -- first release: 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace NHAWK
{
    public class RRDCommandException : Exception
    {
        public RRDCommandException(string message)
            : base(message)
        { }

        public RRDCommandException(string message, Exception ex)
            : base(message, ex)
        { }
    };

    public class NHawkCommand
    {
        static NHawkCommand _instance = null;
        string              _command_path = "rrdtool";

        public static NHawkCommand Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NHawkCommand();

                return _instance;
            }
        }

        private NHawkCommand()
        { }

        public string RRDCommandPath
        {
            set
            {
                if (!File.Exists(value))
                    throw new RRDCommandException("Could not find: " + value);

                _command_path = value;
            }         
           
            get { return _command_path; }
        }

        public void RunCommand(string arguments, bool isSetWorkDir = false)
        {
            Process p;
            //string stdout; // = "";
            string stderr = "";
            try
            {
                p = new Process();
                p.StartInfo.FileName = RRDCommandPath;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.EnvironmentVariables["path"] = RRDCommandPath; 

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;

                if (isSetWorkDir)
                {
                    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(RRDCommandPath);
                }

                //p.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                //if (!System.IO.File.Exists(p.StartInfo.FileName))
                //    throw new Exception("File: " + p.StartInfo.FileName + " not found");

                if (p.Start())
                {
                    //stdout = p.StandardOutput.ReadToEnd();
                    stderr = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                }
                if (p != null)
                {
                    p.Close();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new RRDCommandException("RRDtool Command: ", ex);
            }

            //if (stdout != "")
            //   throw new Exception("RRDTool reported the following error:" + stdout);

            if (stderr != "")
                throw new RRDCommandException(stderr);
        }


        public Stream RunCommandRedirect(string command)
        {
            Process p;
            string stdout = "";
            string stderr = "";
            try
            {
                p = new Process();

                p.StartInfo.FileName = RRDCommandPath;
                p.StartInfo.Arguments = command;
                p.StartInfo.EnvironmentVariables["path"] += "e:\\rrdtool\\release\\";

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                //p.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();

                //if (!System.IO.File.Exists(p.StartInfo.FileName))
                //    throw new Exception("File: " + p.StartInfo.FileName + " not found");

                if (p.Start())
                {
                    stdout = p.StandardOutput.ReadToEnd();
                    stderr = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                }
                if (p != null)
                {
                    p.Close();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new RRDCommandException("Command Command Error: ", ex);
            }


            if (stderr != "")
                throw new RRDCommandException("Command Command Error: " +stderr);

            ASCIIEncoding enc = new ASCIIEncoding();
            MemoryStream mstream = new MemoryStream(enc.GetBytes(stdout));
            return mstream;
        }
    }
       

       public class TestNHawk
       {
#if TEST_NHawkCommand
           public static void Main()
           {
               NHawkCommand nhawk = NHawkCommand.Instance;
               //nhawk.RunCommand("notepad.exe", "");

           }
#endif 

       }
    
}
