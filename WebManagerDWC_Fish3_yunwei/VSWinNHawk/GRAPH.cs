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
//File       : GRAPH.cs   -- provides .Net / Mono wrapper around the RRDTool Graph command construct        //
//Application: NHawk: Open Source Project Support                                                           // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                              //
//Author     : Mike Corley, Syracuse University                                                             //
//             mwcorley@syr.edu                                                                             //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
 * Module Operations
 * =================
 * This module constitues a thin wrapper around the RRDtool graph command construct.  
 * Specifically, it provides the C# facilities for building, manipulat, serializing and
 * deserializing RRDtool graphs. See the RRDtool docs. for more information:
 * http://oss.oetiker.ch/rrdtool/doc/rrdgraph.en.html
 * 
 * 
 * 
 * Public Interface
 * =================
 * public GRAPH(string filename, string start_time, string end_time);
 * -- constructor: 
 *     filename  :  the name of the image file to which the time series is rendered. (RRDTool recommends: .png, .svg, or .eps) not enforced
 *     start_time:  the starting time for the time series (can be specified in several formats: see rrdtool docs.)
 *     end_time  :  the ending time for the time series (can be specififed in several formats: see rrdtool docs.)
 *     
 * public GRAPH(string filename, string start_time, string end_time, DEF def, IGRAPH_ELEMENT elem);
 *  -- constructor: 
 *     filename   :  the name of the image file to which the time series is rendered. (RRDTool recommends: .png, .svg, or .eps) not enforced
 *     start_time :  the starting time for the time series (can be specified in several formats: see rrdtool docs.)
 *     end_time   :  the ending time for the time series (can be specififed in several formats: see rrdtool docs.)
 *     def        :  a default (initial) data definition 
 *     elem       :  a default (initial)  graph element
 *    
 * public GRAPH(string filename, string start_time, string end_time, List<DEF> def_list, List<IGRAPH_ELEMENT> elem_list);
 * -- constructor: 
 *    filename   :  the name of the image file to which the time series is rendered. (RRDTool recommends: .png, .svg, or .eps) not enforced
 *    start_time :  the starting time for the time series (can be specified in several formats: see rrdtool docs.)
 *    end_time   :  the ending time for the time series (can be specififed in several formats: see rrdtool docs.)
 *    def_list   :  list of externally defined data definitions
 *    elem_list  :  list of externally graph elements
 * 
 * public GRAPH(string filename, string start_time, string end_time, CDEF cdef, GRAPH_ELEMENT elem);
 * -- constructor: 
 *    filename   : the name of the image file to which the time series is rendered. (RRDTool recommends: .png, .svg, or .eps) not enforced
 *    start_time : the starting time for the time series (can be specified in several formats: see rrdtool docs.)
 *    end_time   : the ending time for the time series (can be specififed in several formats: see rrdtool docs.)
 *    cdef       : a default (initial) data calculation definition 
 *    elem       : a default (initial) graph element
 *   
 * public GRAPH(string filename, string start_time, string end_time, List<CDEF> cdef_list, List<GRAPH_ELEMENT> elem_list);
 * -- constructor: 
 *    filename   :  the name of the image file to which the time series is rendered. (RRDTool recommends: .png, .svg, or .eps) not enforced
 *    start_time :  the starting time for the time series (can be specified in several formats: see rrdtool docs.)
 *    end_time   :  the ending time for the time series (can be specififed in several formats: see rrdtool docs.)
 *    def_list   :  list of externally defined data definitions
 *    elem_list  :  list of externally defined graph elements
 * 
 * public GRAPH(string filename, string start_time, string end_time, List<DEF> def_list, List<CDEF> cdef_list, List<GRAPH_ELEMENT> elem_list);
 * -- constructor: 
 *    filename  :  the name of the image file to which the time series is rendered. (RRDTool recommends: .png, .svg, or .eps) not enforced
 *    start_time:  the starting time for the time series (can be specified in several formats: see rrdtool docs.)
 *    end_time  :  the ending time for the time series (can be specififed in several formats: see rrdtool docs.)
 *    cdef_list :  list of externally defined data definitions
 *    def_list  :  list of externally defined data definitions
 *    elem_list :  list of externally defined graph elements
 * 
 * public GRAPH(string graph, string workingdir);
 * -- deserialization constructor
 *    graph: a raw string (serialized) representation of a graph object.  if successful, a fully initialized
 *           graph object is constructed, other an appropriate exception is thrown. 
 *           RRDFileNotFoundException  -- thrown if an RRD file referenced from with in a DEF cannot be found 
 *           RRDFormatException        -- thrown if the RRD format is not valid and cant be deserialized
 *           RRAFormatException        -- thrown if a RRA format is is not valid and cant be deserialized
 *           DSFormatException         -- 
 *           DataSourceException)  
 *           XGridParameterException
 *           CDEF_Exception
 *           RRACFException
 *           DefException
 *           GraphElementFormatException
 *           
 * public xgrid(XGrid.GTYPE t)
 * --sets the XAxis labeling mode 
 *        set t == XGrid.GTYPE.AUTO to turn autoconfiguration.  Note: this option is set by default, 
 *        so if no specification is made for the XAxis, then NHAWK will ask RRDTool to use 
 *        default autoconfiguration.
 *       
 *        set t == XGrid.GTYPE.NONE to turn off (suppress) the XAxis labeling completely
 * 
 * public void  xgrid(XGrid.TM GTM, int GST, XGrid.TM MTM, int MST, XGrid.TM LTM, int LST, int LPR, string LFM)
 * -- manually specifies the XAxis label
 *    The grid is defined by specifying a certain amount of time in the ?TM positions. You can choose from SECOND, MINUTE, HOUR, DAY, WEEK, MONTH or YEAR. Then you define 
 *    how many of these should pass between each line or label. This pair (?TM:?ST) needs to be 
 *    specified for the base grid (G??), the major grid (M??) and the labels (L??). For the labels 
 *    you also must define a precision in LPR and a strftime format string in LFM. LPR defines where 
 *    each label will be placed. If it is zero, the label will be placed right under the corresponding
 *    line (useful for hours, dates etcetera). If you specify a number of seconds here the label is 
 *    centered on this interval (useful for Monday, January etcetera).
 *    
 *    e.g.  xgrid(XGrid.TM.MINUTE, 10, XGrid.TM.HOUR, 1, XGrid.TM.HOUR, 1, 0, "%X");
 *    
 *          This places grid lines every 10 minutes, major grid lines every hour, 
 *          and labels every 4 hours. The labels are placed under the major grid lines 
 *          as they specify exactly that time.
 *          
 *    e.g.  xgrid(XGrid.TM.HOUR, 8, XGrid.TM.DAY, 1, XGrid.TM.DAY, 1, 86400, "%A");
 *          
 *          This places grid lines every 8 hours, major grid lines and labels each day. 
 *          The labels are placed exactly between two major grid lines as they specify the 
 *          complete day and not just midnight
 *
 * public string xgrid_ToString()
 * -- returns the serialized (string) representation of the current XAxis grid specification
 * 
 * public void ygrid(YGrid.GTYPE t)
 * --sets the YAxis labeling mode
 *        set t == YGrid.GTYPE.AUTO to turn on autoconfiguration.  Note: this option is set by default, 
 *        so if no specification is made for the YAxis, then NHAWK will ask RRDTool to use 
 *        default autoconfiguration.
 *       
 *        set t == XGrid.GTYPE.NONE to turn off (suppress) the YAxis labeling completely
 *        
 * public void ygrid(double grid_step, double label_factor)
 * -- manually specifies the YAxis label 
 *    Y-axis grid lines appear at each grid_step interval. 
 *    Labels are placed every label_factor lines
 * 
 * public string   ygrid_ToString()
 *  -- returns the serialized (string) representation of the current YAxis grid specification
 * 
 * public bool     no_gridfit
 * -- sets anti-aliasing blurring effects rrdtool snaps points to device resolution pixels, 
 *    this results in a crisper apearance. If this is not to your liking, you can use this 
 *    switch to turn this behaviour off.
 *    set == true to switch on (ask rrdtool not to grid fit)
 *    set == false (set by default) to allow gridfitting
 *    
 * public bool     autoscale
 * -- when set == true this option determines the values used for autoscaleing the YAxix
 *    by calculating the minimum and maximum y-axis from the actual minimum and maximum 
 *    data values
 *    set == false (set by default) to use the default autoscaling algorithm
 *    
 * public bool     autoscale_min
 * -- when set == true this option will modify both the absolute maximum AND minimum values, 
 *    this option will only affect the minimum value. The maximum value, if not defined on the 
 *    command line, will be 0. This option can be useful when graphing router traffic when the 
 *    WAN line uses compression, and thus the throughput may be higher than the WAN line speed.
 *    set == false (set by default) to turn off this option
 *    
 * public bool     autoscale_max
 * -- when set == true this option will modify both the absolute maximum AND minimum values, 
 *    this option will only affect the maximum value. The minimum value, if not defined on the 
 *    command line, will be 0. This option can be useful when graphing router traffic when the 
 *    WAN line uses compression, and thus the throughput may be higher than the WAN line speed.
 *    set == false (set by default) to turn off this option
 * 
 * public bool     onlygraph
 * -- set == true and set the height < 32 pixels you will get a tiny graph image (thumbnail) 
 *    to use as an icon for use in an overview, for example. All labeling will be stripped off 
 *    the graph.
 *    set == false by default
 *
 * public double   ylowerlimit
 * public double   yupperlimit
 * public bool     rigid
 * -- By default the graph will be autoscaling so that it will adjust the y-axis to the range 
 *    of the data. You can change this behaviour by explicitly setting the limits. 
 *    The displayed y-axis will then range at least from lower-limit to upper-limit. 
 *    Autoscaling will still permit those boundaries to be stretched unless the rigid option 
 *    is set.  All are set to "off" by default.  To explicity turn off ylowerlimit and yupperlimit
 *    use: ylowerlimit=Double.NaN and yupperlimit=Double.NaN respectively, and use: rigid=false
 *    for the rigid option.
 *    
 * public bool     logarithmic
 * -- set == true to cause the Yaxis to be logarithmically scaled.
 *    set == false by default
 *   
 * public bool     full_size_mode
 * -- when set == true the width and height specify the final dimensions of the output
 *    image and the canvas is automatically resized to fit.  set == false by default
 *    
 * public bool     alt_y_grid 
 * -- when set == true places the Y grid dynamically based on the graph's Y range. The 
 *    algorithm ensures that you always have a grid, that there are enough but not too 
 *    many grid lines, and that the grid is metric. set == false by default
 *    
 * public bool     units_si     
 * -- when set == true y-axis values on logarithmic graphs will be scaled to the 
 *    appropriate units (k, M, etc.) instead of using exponential notation. 
 *    Note that for linear graphs, SI notation is used by default.  set == false by
 *    default
 *
 * public string   filename
 * -- gets or sets complete path of the graph to generate. 
 *    It is recommended to end this in .png, .svg or .eps
 *    
 * public string   yaxislabel
 * -- gets or sets the yaxis title. Vertically placed string at the left hand side of the graph.
 *
 * public string   title
 * -- gets or sets the title of the graph. A horizontal string at the top of the graph 
 *  
 * public long     step
 * -- gets or sets the step. rrdtool graph calculates the width of one pixel in the time domain 
 * and tries to get data from an RRA with that resolution. With the step option you can alter 
 * this behavior. If you want rrdtool graph to get data at a one-hour resolution from the RRD, 
 * set step to 3'600. Note: a step smaller than one pixel will silently be ignored.
 
 * public string   start
 * -- gets or sets the starting time for the time series 
 *    (can be specified in several formats: see rrdtool docs.)
 *    
 * public string   end
 * -- gets or sets the the ending time for the time series 
 *   (can be specififed in several formats: see rrdtool docs.)
 *
 * public int      height
 * -- gets or sets the height of the graph in pixels
 *    set == 0 for rrdtool to use default size
 *    By default, the width and height of the canvas 
 *    (the part with the actual data and such). 
 *    This defaults to 400 pixels by 100 pixels.
 * 
 * public int      width
 * -- gets or sets the width of the graph in pixels
 *    set == 0 for rrdtool to use default size
 *    By default, the width and height of the canvas 
 *    (the part with the actual data and such). 
 *    This defaults to 400 pixels by 100 pixels
 *    
 * public int units_exponent
 * -- This sets the 10**exponent scaling of the y-axis values. Normally, 
 *    values will be scaled to the appropriate units (k, M, etc.). 
 *    However, you may wish to display units always in k (Kilo, 10e3) 
 *    even if the data is in the M (Mega, 10e6) range, for instance. 
 *    Value should be an integer which is a multiple of 3 between -18 and 18 
 *    inclusively. It is the exponent on the units you wish to use. For example, 
 *    use 3 to display the y-axis values in k (Kilo, 10e3, thousands), use -6 to 
 *    display the y-axis values in u (Micro, 10e-6, millionths). Use a value of 0 
 *    to prevent any scaling of the y-axis values.
 *
 *    set == int.MaxValue to turn off this feature (set by default)
 *
 * public int units_length  
 * -- gets or sets the How many digits should rrdtool assume the y-axis labels to be? 
 *    You may have to use this option to make enough space once you start fideling with 
 *    the y-axis labeling.
 *    set == int.MaxValue to turn off this feature (set by default)
 *    
 * public int zoom      
 * -- gets or sets the zoom factor zoom the graphics by the specified the amount. 
 *    The factor must be > 0
 *    
 * public void addCDEF(CDEF cdef)
 * -- adds a CDEF to the current graph 
 *  
 * public void addDEF(DEF def)
 * -- add a DEF to the current graph
 *  
 * public void addGELEM(IGRAPH_ELEMENT elem)
 * -- add a graph element to the current graph
 *
 * public static GRAPH DeepClone(GRAPH gr);
 * -- returns a new memory copy of gr 
 *     -- graphs ( and all NHAWK constructs) are treated as composite structures with which every element of the graph knows how to 
 *        DeepClone() itself.
 *        Note on DeepCloning: In the NHAWK model, the DeepClone works in the shallow reference model because every member
 *        member stored is either value type or string reference type.  New string objects are construced, initialized and 
 *        returned through the use of a temporary StringBuilder() object. The StringBuilder's ToString() method is called to 
 *        yield the new string instance.  This is useful to keep graph definitions from being modified through external references.   
 * 
 * public override string ToString()
 * -- returns the serialized (string) representation of the graph
 * 
 * public void graph()
 * -- invokes the the rrdtool executable to process the graph command
 * 
 * Build Process
 * =============
 * 
 * This module can be compiled stand alone for testing purposes: 
 * Note: some modules have additional assemblies dependencies which
 *      are needed for compilation of the test stub.  The required files section
 *      shows true dependency structure. i.e when the test stub is not compiled.
 *
 * Compiler Command:
 * Visual Studio 2008 (.Net 3.5): csc /define:TEST_GRAPH /reference:c:\windows\microsoft.net\framework\v2.0.50727\Microsoft.VisualBasic.dll  
 *                                GRAPH.cs ..\NHawkCommand\NHawkCommand.cs ..\DEF\DEF.cs ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs  ..\RRD\RRD.cs 
 *                                ..\CDEF\CDEF.cs ..\RRA\RRA.cs ..\DS\DS.cs ..\GRAPH_ELEMENT_FACTORY\GRAPH_ELEMENT_FACTORY.cs
 *							      ..\LINE\LINE.cs ..\AREA\AREA.cs ..\SHIFT\SHIFT.cs ..\HRULE\HRULE.cs ..\VRULE\VRULE.cs
 *    
 * Win32 Mono (1.9.1)           : gmcs -define:TEST_GRAPH -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.Drawing.dll 
 *                                                       -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\Microsoft.VisualBasic.dll
 *                                                       -r:"C:\Program Files"\Mono-1.9.1\lib\mono\2.0\System.dll 
 *                                                        GRAPH.cs ..\NHawkCommand\NHawkCommand.cs ..\DEF\DEF.cs ..\GRAPH_ELEMENT\GRAPH_ELEMENT.cs  
 *                                                        ..\RRD\RRD.cs ..\CDEF\CDEF.cs  ..\RRA\RRA.cs ..\DS\DS.cs ..\GRAPH_ELEMENT_FACTORY\GRAPH_ELEMENT_FACTORY.cs 
 *                                                        ..\LINE\LINE.cs ..\AREA\AREA.cs ..\SHIFT\SHIFT.cs ..\HRULE\HRULE.cs ..\VRULE\VRULE.cs
 *
 *                                                                   
 * SUSE Linux 10.3 Mono 1.9.1   : gmcs -define:TEST_GRAPH -r:/usr/lib/mono/gac/System/2.0.0.0__b77a5c561934e089/System.dll 
 *                                                        -r:/usr/lib/mono/gac/System.Drawing/2.0.0.0__b03f5f7f11d50a3a/System.Drawing.dll 
 *                                                        -r:/usr/lib/mono/gac/Microsoft.VisualBasic/8.0.0.0__b03f5f7f11d50a3a/Microsoft.VisualBasic.dll  
 *                                                        GRAPH.cs ../NHawkCommand/NHawkCommand.cs ../DEF/DEF.cs ../GRAPH_ELEMENT/GRAPH_ELEMENT.cs  
 *                                                        ../RRD/RRD.cs ../CDEF/CDEF.cs  ../RRA/RRA.cs ../DS/DS.cs ../GRAPH_ELEMENT_FACTORY/GRAPH_ELEMENT_FACTORY.cs 
 *                                                        ../LINE/LINE.cs ../AREA/AREA.cs ../SHIFT/SHIFT.cs ../HRULE/HRULE.cs ../VRULE/VRULE.cs
 *
 * 
 * Required Files:  GRAPH.cs DEF.cs CDEF.ds RRD.cs RRA.cs DS.cs NHawkCommand.cs GRAPH_ELEMENT.cs 
 *                  GRAPH_ELEMENT_FACTORY.cs LINE.cs AREA.cs SHIFT.cs HRULE.cs VRULE.cs
 *
 * 
 * Example Usage
 * =============
 * See Test stub below
 * 
 * Maintanence History
 * ===================
 * version 1.0 : 01 August 08 
 *     -- first release: 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Text;

namespace NHAWK
{
    
    [Serializable]
    public class GraphFormatException : Exception
    {
        public GraphFormatException(string message)
            : base(message)
        { }

        public GraphFormatException(string message, Exception ex)
            : base(message, ex)
        { }
    };


    [Serializable]
    public class XGridParameterException : Exception
    {
        public XGridParameterException(string message)
            : base(message)
        { }
    };

   
    //class for handling the XGrid option
    [Serializable]
    public class XGrid
    {
        private TM        _gtm;
        private TM        _mtm;
        private TM        _ltm;
        private int       _gst;
        private int       _mst;
        private int       _lst;
        private int       _lpr;
        private string    _lfm;
        private const int TOKEN_COUNT = 8;

        public enum GTYPE 
        {
            NONE, AUTO
        };

        public enum TM
        {
            SECOND, MINUTE, HOUR, DAY, WEEK, MONTH, YEAR
        };

        public static int token_count
        {
           get { return TOKEN_COUNT; }
        }

        public XGrid(TM GTM, int GST, TM MTM, int MST, TM LTM, int LST, int LPR, string LFM)
        {
            this.GTM = GTM; 
            this.GST = GST;
            this.MTM = MTM;
            this.MST = MST;
            this.LTM = LTM;
            this.LST = LST;
            this.LPR = LPR;
            this.LFM = LFM;
        }

        public XGrid(GTYPE t)
        {
            if(t == GTYPE.NONE)
            {
                this.GTM = TM.SECOND;
                this.GST = 0;
                this.MTM = TM.SECOND;
                this.MST = 0;
                this.LTM = TM.SECOND;
                this.LST = 0;
                this.LPR = 0;
                this.LFM = "NONE";
            }
            else
            {
                this.GTM = TM.SECOND;
                this.GST = 0;
                this.MTM = TM.SECOND;
                this.MST = 0;
                this.LTM = TM.SECOND;
                this.LST = 0;
                this.LPR = 0;
                this.LFM = "AUTO";
            }
        }

        bool verify(string token)
        {
            string tok = token.ToUpper();

            if (tok == "MINUTE" || tok == "HOUR" || tok == "SECOND" ||
                tok == "DAY" || tok == "WEEK" || tok == "MONTH" ||
                tok == "YEAR")
                return true;

            return false;
        }


        public TM StringToTM(string tm)
        {
            switch (tm.ToUpper())
            {
                case "MINUTE":
                    return TM.MINUTE;
                case "DAY":
                    return TM.DAY;
                case "HOUR":
                    return TM.HOUR;
                case "SECOND":
                    return TM.SECOND;
                case "WEEK":
                    return TM.WEEK;
                case "MONTH":
                    return TM.MONTH;
                case "YEAR":
                    return TM.YEAR;
                default:
                    throw new XGridParameterException("Undefined TM value: " + tm.ToUpper() + " ?");
            }
        }

        public XGrid(string[] xgrid)
        {
            if (xgrid.Length != TOKEN_COUNT)
                throw new XGridParameterException("Bad Parameter count");

            if (verify(xgrid[0]) && verify(xgrid[2]) && verify(xgrid[4]))
            {
                this.GTM = StringToTM(xgrid[0]);
                this.GST = int.Parse(xgrid[1]);
                this.MTM = StringToTM(xgrid[2]);
                this.MST = int.Parse(xgrid[3]);
                this.LTM = StringToTM(xgrid[4]);
                this.LST = int.Parse(xgrid[5]);
                this.LPR = int.Parse(xgrid[6]);
                this.LFM = xgrid[7];
            }
            else
                throw new XGridParameterException("Expected TM value ?");
        }


        public override string ToString()
        {
            if (this.LFM == "NONE")
                return "none";
            else if (this.LFM == "AUTO")
                return "";
            else
               return GTM + ":" + GST + ":" + MTM + ":" + MST + ":" + LTM + ":" + LST + ":" + LPR + ":" + LFM;
        }


        public static XGrid DeepClone(XGrid xgrid)
        {
            return new XGrid(xgrid.GTM, xgrid.GST, xgrid.MTM, xgrid.MST, xgrid.LTM, xgrid.LST, 
                             xgrid.LPR, ((new StringBuilder()).Append(xgrid.LFM)).ToString());
        }

        public TM LTM
        {
            get { return _ltm;  }
            set { _ltm = value; }
        }

        public TM MTM
        {
            get { return _mtm;  }
            set { _mtm = value; }
        }

        public TM GTM
        {
            get { return _gtm;  }
            set { _gtm = value; }
        }

        public int MST
        {
            get { return _mst; }
            set
            {
                if (value < 0)
                    throw new XGridParameterException("MST cannot be negative: " + value);
                _mst = value;
            }
        }

        public int GST
        {
            get { return _gst; }
            set
            {
                if (value < 0)
                    throw new XGridParameterException("GST cannot be negative: " + value);
                _gst = value;
            }
        }

        public int LST
        {
            get { return _lst; }
            set
            {
                if (value < 0)
                    throw new XGridParameterException("LST cannot be negative: " + value);
                _lst = value;
            }
        }

        public int LPR
        {
            get { return _lpr; }
            set
            {
                if (value < 0)
                    throw new XGridParameterException("LPR cannot be negative: " + value);

                _lpr = value;
            }
        }

        public string LFM
        {
            get { return _lfm; }
            set
            {
                  //  throw new XGridParameterException("LFM cannot be negative: " + value);
                _lfm = value;
            }
        }
    }

    //class for handling the YGrid option
    [Serializable]
    public class YGrid
    {
        private double step_;
        private double label_factor_;  
       

        public enum GTYPE
        {
            NONE, AUTO
        };

        public YGrid(double step, double label_factor)
        {
            this.step = step;
            this.label_factor = label_factor;
        }


        public YGrid(string ygrid)
        {
            string[] args = ygrid.Split(new char[] { ':' });
            step = Double.Parse(args[0]);
            label_factor =  double.Parse(args[1]);
        }

        public YGrid(GTYPE t)
        {
            if (t == GTYPE.NONE)
            {
                step = 0;
                label_factor = Double.NaN;
            }
            else
            {
                step = Double.NaN;
                label_factor = 0;
            }
        }


        public override string ToString()
        {
            if(Double.IsNaN(this.label_factor) && step == 0)
              return "none";
            else if(label_factor == 0 && Double.IsNaN(step))
              return "";

            return step + ":" + label_factor.ToString();
        }


        public static YGrid DeepClone(YGrid ygrid)
        {
            return new YGrid(ygrid.step, ygrid.label_factor);
        }

        public double step
        {
            get { return step_; }
            set { step_ = value; }
        }
       
        public double label_factor
        {
            get { return label_factor_; }
            set { label_factor_ = value; }
        }
    } 
   

    [Serializable]
    public class GRAPH
    {
        string               _start_time;
        string               _end_time;
        long                 _step;
        double               _y_upper_limit;
        double               _y_lower_limit;
        List<DEF>            _def_list;
        List<CDEF>           _cdef_list;
        List<IGRAPH_ELEMENT> _elem_list;
        XGrid                _xgrid;
        YGrid                _ygrid;
        string               _name;
        string               _title;
        string               _y_axix_label;
        int                  _height;
        int                  _width;
        int                  _units_exponent;
        int                  _units_length;
        int                  _zoom;
        bool                 _units_si;
        bool                 _logarithmic;
        bool                 _only_graph;
        bool                 _rigid;
        bool                 _autoscale;
        bool                 _autoscale_min;
        bool                 _autoscale_max;
        bool                 _no_gridfit;
        bool                 _full_size_mode;
        bool                 _alt_y_grid;

        private GRAPH()
        {
            filename       = "rrd_default.png"; 
            title          = "";
            yaxislabel     = "";
            start          = "0";
            end            = "0";
            width          = 0;
            height         = 0;
            step           = 0;
            units_exponent = int.MaxValue;
            units_length   = int.MaxValue;
            zoom           = int.MaxValue;

            yupperlimit    = Double.NaN;
            ylowerlimit    = Double.NaN;
            logarithmic    = false;
            onlygraph      = false;
            rigid          = false;
            autoscale      = false;
            autoscale_min  = false;
            autoscale_max  = false;
            no_gridfit     = false;
            full_size_mode = false;
            alt_y_grid     = false;
            units_si       = false;

            _xgrid = new XGrid(XGrid.GTYPE.AUTO);
            _ygrid = new YGrid(YGrid.GTYPE.AUTO);

            _def_list = new List<DEF>();
            _cdef_list = new List<CDEF>();
            _elem_list = new List<IGRAPH_ELEMENT>();

            TitleSize = 20;
            FontName = "simkai.ttf";
        }


        public GRAPH(string fname, string start_time, string end_time)
            : this()
        {
            this.filename = fname;
            start = start_time;
            end = end_time;
        }

        public GRAPH(string fname, string start_time, string end_time, DEF def, IGRAPH_ELEMENT elem)
            : this(fname, start_time, end_time)
        {
            bind(def, elem);
        }

        public GRAPH(string fname, string start_time, string end_time, List<DEF> def_list, List<IGRAPH_ELEMENT> elem_list)
            : this(fname, start_time, end_time)
        {
            _def_list = def_list;
            _elem_list = elem_list;

            //bind(def, elem);
        }

        public GRAPH(string fname, string start_time, string end_time, CDEF cdef, IGRAPH_ELEMENT elem)
            : this(fname, start_time, end_time)
        {
            bind(cdef, elem);
        }

        public GRAPH(string fname, string start_time, string end_time, List<CDEF> cdef_list, List<IGRAPH_ELEMENT> elem_list)
            : this(fname, start_time, end_time)
        {
            _cdef_list = cdef_list;
            _elem_list = elem_list;

            //bind(def, elem);
        }

        public GRAPH(string fname, string start_time, string end_time, List<DEF> def_list, List<CDEF> cdef_list, List<IGRAPH_ELEMENT> elem_list)
            : this(fname, start_time, end_time)
        {
            _def_list  = def_list;
            _cdef_list = cdef_list;
            _elem_list = elem_list;

            //bind(def, elem);
        }

        public GRAPH(string graph, string workingdir): this()
        {
            string gr = GRAPH.PreSerializeFormat(graph);
            
            string[] first_level_tokens; string _cwd;
            _cwd = Directory.GetCurrentDirectory();
            try
            {
                first_level_tokens = gr.Split(new char[] { ' ', '\t' });
                //RemoveSlashes(first_level_tokens);
                this._def_list = new List<DEF>();  this._elem_list = new List<IGRAPH_ELEMENT>();

                if (first_level_tokens[0].Trim().ToLower() != "graph")
                    throw new GraphElementFormatException("Bad create \"graph\" command syntax");
                else
                    this.filename = first_level_tokens[1].Trim();

                for (int i = 2; i < first_level_tokens.Length; i++)
                {
                    OptionsParser(first_level_tokens, ref i);

                    //parse out graph definition
                    if (first_level_tokens[i].Trim().ToUpper().IndexOf("DEF") == 0)
                    {
                        Directory.SetCurrentDirectory(workingdir);
                        this._def_list.Add(new DEF(first_level_tokens[i].Trim()));
                        continue;
                    }

                    if (first_level_tokens[i].Trim().ToUpper().IndexOf("CDEF") == 0)
                    {
                        Directory.SetCurrentDirectory(workingdir);
                        this._cdef_list.Add(new CDEF(first_level_tokens[i].Trim()));
                        continue;
                    }

                    //parse out and create graph elements using Graph_Element object factory
                    IGRAPH_ELEMENT graph_element;
                    if ((graph_element = GRAPH_ELEMENT_FACTORY.CreateElement(first_level_tokens[i].Trim())) != null)
                        this._elem_list.Add(graph_element);
                }

                PostSerializeFormat();
            }
            catch (Exception ex)
            {
                if ((ex is RRDFileNotFoundException) || (ex is RRDFileExistsException) ||
                    (ex is RRDFormatException) || (ex is RRAFormatException)  ||
                    (ex is DSFormatException) || (ex is DataSourceException)  ||
                    (ex is XGridParameterException) || (ex is CDEF_Exception) ||
                    (ex is RRACFException) || (ex is DefException) || (ex is GraphElementFormatException))
                    throw;
                else
                    throw new GraphElementFormatException("Bad RRDtool \"Graph\" command syntax:", ex);
            }
            finally
            {
                Directory.SetCurrentDirectory(_cwd);
            }
        }


        public static GRAPH DeepClone(GRAPH gr)
        {
            GRAPH gtemp = new GRAPH(((new StringBuilder()).Append(gr.filename)).ToString(),
                                     ((new StringBuilder()).Append(gr.start)).ToString(),
                                     ((new StringBuilder()).Append(gr.end)).ToString());
            
            gtemp.width          = gr.width;
            gtemp.height         = gr.height;
            gtemp.logarithmic    = gr.logarithmic;
            gtemp.title          = ((new StringBuilder()).Append(gr.title)).ToString();
            gtemp.yaxislabel     = ((new StringBuilder()).Append(gr.yaxislabel)).ToString();
            gtemp.full_size_mode = gr.full_size_mode;
            gtemp.units_si       = gr.units_si;
            gtemp.autoscale      = gr.autoscale;
            gtemp.autoscale_min  = gr.autoscale_min;
            gtemp.autoscale_max  = gr.autoscale_max;
            gtemp.onlygraph      = gr.onlygraph;
            gtemp.yupperlimit    = gr.yupperlimit;
            gtemp.ylowerlimit    = gr.ylowerlimit;
            gtemp.no_gridfit     = gr.no_gridfit;
            gtemp.rigid          = gr.rigid;
            gtemp.step           = gr.step;
            gtemp.alt_y_grid     = gr.alt_y_grid;
            gtemp.units_exponent = gr.units_exponent;
            gtemp.units_length   = gr.units_length;
            gtemp.zoom           = gr.zoom;
            gtemp._ygrid         = YGrid.DeepClone(gr._ygrid);
            gtemp._xgrid         = XGrid.DeepClone(gr._xgrid);
            
            foreach (DEF def in gr._def_list)
                gtemp.addDEF(DEF.DeepClone(def));

            foreach (CDEF cdef in gr._cdef_list)
                gtemp.addCDEF(CDEF.DeepClone(cdef));

            foreach (IGRAPH_ELEMENT elem in gr._elem_list)
                gtemp.addGELEM(elem.DeepClone());

            return gtemp;
        }

        public void xgrid(XGrid.TM GTM, int GST, XGrid.TM MTM, int MST, XGrid.TM LTM, int LST, int LPR, string LFM)
        {
            _xgrid = new XGrid(GTM, GST, MTM, MST, LTM, LST, LPR, LFM);
        }

        public void xgrid(XGrid.GTYPE t)
        {
            _xgrid = new XGrid(t);
        }

        public string xgrid_ToString()
        {
            return _xgrid.ToString(); 
        }

        public void ygrid(double grid_step, double label_factor)
        {
            _ygrid = new YGrid(grid_step, label_factor);
        }

        public void ygrid(YGrid.GTYPE t)
        {
            _ygrid = new YGrid(t);
        }

        public string ygrid_ToString()
        {
            return _ygrid.ToString();
        }

        public int units_exponent
        {
            get { return _units_exponent; }
            set
            {
                if (value > 18 &&  (value != int.MaxValue) || (value < -18) || (((value % 3) != 0) &&  (value != int.MaxValue)))
                    throw new GraphElementFormatException("units-exponent option value should  should be an integer which is a multiple of 3 between -18 and 18 inclusively");
                _units_exponent = value;
            }
        }


        public int zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        public bool units_si
        {
            get { return _units_si; }
            set { _units_si = value; }
        }

        public int units_length
        {
            get { return _units_length; }
            set { _units_length = value; }
        }

        public bool alt_y_grid
        {
            get { return _alt_y_grid; }
            set { _alt_y_grid = value; }
        }
        
        public bool no_gridfit
        {
            get { return _no_gridfit; }
            set { _no_gridfit = value; }
        }

        public bool autoscale
        {
            get { return _autoscale; }
            set { _autoscale = value; }
        }

        public bool autoscale_min
        {
            get { return _autoscale_min; }
            set { _autoscale_min = value; }
        }

        public bool autoscale_max
        {
            get { return _autoscale_max; }
            set { _autoscale_max = value; }
        }

        public bool onlygraph
        {
            get { return _only_graph; }
            set { _only_graph = value; }
        }

        public bool full_size_mode
        {
            get { return _full_size_mode; }
            set { _full_size_mode = value; }
        }

        public double ylowerlimit
        {
            get { return _y_lower_limit; }
            set { _y_lower_limit = value; }
        }

        public double yupperlimit
        {
            get { return _y_upper_limit; }
            set { _y_upper_limit = value; }
        }

        public bool rigid
        {
            get { return _rigid; }
            set { _rigid = value; }
        }

        public bool logarithmic
        {
            get { return _logarithmic; }
            set { _logarithmic = value; }
        }

        public long step
        {
            get { return _step; }
            set
            {
                if (value >= 0)
                    _step = value;
            }
        }

        public string yaxislabel
        {
            get { return _y_axix_label; }
            set {_y_axix_label = value; }
        }


        public string title
        {
            get { return _title;  }
            set { _title = value; }
        }

        public string FontName
        {
            set;
            get;
        }
        public int TitleSize { set; get; }

       
        public int height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                    throw new GraphFormatException("height property cannot be negative: " + value.ToString());
                _height = value;
            }
        }


        public int width
        {
            get { return _width; }
            set
            {
                if (value < 0)
                    throw new GraphFormatException("width cannot be negative: " + value.ToString());
                _width = value;
            }
        }


        public void bind(CDEF cdef, IGRAPH_ELEMENT elem)
        {
            //if (def.name == elem.val)
            {
                this.addCDEF(cdef);
                addGELEM(elem);
            }
            //  else
            //    throw new GraphFormatException("graph element must reference a valid graph definition: " + def.name + " != " + elem.val);
        }

        public void bind(DEF def, IGRAPH_ELEMENT elem)
        {
            //if (def.name == elem.val)
            {
                addDEF(def);
                addGELEM(elem);
            }
          //  else
            //    throw new GraphFormatException("graph element must reference a valid graph definition: " + def.name + " != " + elem.val);
        }


        public void graph(string working_dir)
        {
            string cwd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(working_dir);
                NHawkCommand.Instance.RunCommand(ToString());
            }
            finally
            {
                Directory.SetCurrentDirectory(cwd);
            }
        }

        public void graph()
        {
            NHawkCommand.Instance.RunCommand(ToString(), true);
        }

        public void addCDEF(CDEF cdef)
        {
            if (!ContainsCDEF(cdef.vname))
                _cdef_list.Add(cdef);
            else
               throw new GraphFormatException("Attempt to add duplicate calc. definition: " + cdef.vname);
        }

        public void addDEF(DEF def)
        {
            if (!ContainsDEF(def.name))
                _def_list.Add(def);
            else
                throw new GraphFormatException("Attempt to add duplicate definition: " + def.name);
        }

        public void addGELEM(IGRAPH_ELEMENT elem)
        {
           // if (ContainsDEF(elem.val))
                _elem_list.Add(elem);
          //  else
           //     throw new GraphFormatException("No Definition named: " + elem.val+ " exists");
        }


       
        public string filename
        {
            get { return _name; }
            set { _name = value; }
        }

        public string start
        {
            get { return _start_time; }
            set { _start_time = value; }
        }

        public string end
        {
            get { return _end_time; }
            set { _end_time = value; }
        }

       
        public override string ToString()
        {
            StringBuilder def_list_builder = new StringBuilder();
            StringBuilder cdef_list_builder = new StringBuilder();
            StringBuilder elem_list_builder = new StringBuilder();

            foreach (DEF def in _def_list)
                def_list_builder.Append(" " + def.ToString());

            foreach (CDEF cdef in _cdef_list)
                cdef_list_builder.Append(" " + cdef.ToString());

            foreach (IGRAPH_ELEMENT elem in _elem_list)
                elem_list_builder.Append(" " + elem.ToString());

            string unitslen   = (units_length != int.MaxValue)   ? " --units-length "   + units_length.ToString() : ""; 
            string unitsexp   = (units_exponent != int.MaxValue) ? " --units-exponent " + units_exponent.ToString() : "";
            string z          = (zoom != int.MaxValue)           ? " --zoom " + zoom : "";                  
            string altygrid   = (alt_y_grid)             ? " --alt-y-grid  " : "";
            string un_si      = (units_si)               ? " --units=si " : "";
            string log_scale  = (logarithmic)            ? " --logarithmic " : "";
            string fsize_mode = (full_size_mode)         ? " --full-size-mode " : "";
            string ngrid      = (no_gridfit)             ? " --no-gridfit " : "";
            string only_graph = (onlygraph)              ? " --only-graph "  : "";
            string auto       = (autoscale)              ? " --alt-autoscale " : "";
            string auto_min   = (autoscale_min)          ? " --alt-autoscale-min " : "";
            string auto_max   = (autoscale_max)          ? " --alt-autoscale-max " : "";
            string w          = (width > 0)              ? " --width " + width.ToString() : "";
            string s          = (step > 0)               ? " --step " + step.ToString() : "";
            string yupper     = (yupperlimit.ToString().ToLower() != "nan") ? " --upper-limit " + yupperlimit.ToString() : "";
            string ylower     = (ylowerlimit.ToString().ToLower() != "nan") ? " --lower-limit " + ylowerlimit.ToString() : "";
            string rig        = (rigid)                  ? " --rigid " : "";
            string h          = (height > 0)             ? " --height " + height.ToString() : "";
            string t          = (title != "")            ? " --title " + "\""+ title + "\"" : "";
            string yaxis      = (yaxislabel != "")       ? " --vertical-label " + "\"" + yaxislabel + "\"" : "";
            string xgridstr   = (xgrid_ToString() != "") ? " --x-grid " + xgrid_ToString() : xgrid_ToString();
            string ygridstr   = (ygrid_ToString() != "") ? " --y-grid " + ygrid_ToString() : ygrid_ToString();

            string options = " " + s + t + xgridstr + ygridstr + yaxis + yupper + ylower + rig + w + h + log_scale + un_si + z 
                                 + unitsexp + unitslen + altygrid + only_graph + auto + auto_min + auto_max + ngrid + fsize_mode;

            string font = " -n TITLE:" + TitleSize + ":" + FontName + " ";
            return "graph " + filename + font + " --start " + start + " --end " + end + options + 
                              def_list_builder.ToString() + cdef_list_builder.ToString() + elem_list_builder.ToString();
        }

        public void export(string path, bool append)
        {
            StreamWriter sw=null;
            try
            {
                sw = new StreamWriter(path, append);
                sw.Write(ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        private string ExtractTitle(string[] first_level_tokens, ref int i)
        {
            StringBuilder tb = new StringBuilder();
            string title = null;

            //do forward closure on title tokens
            if (((i + 1) < first_level_tokens.Length))
            {
                //if a single word title enclosed in quotes
                if ((first_level_tokens[i + 1].Trim()[0] == '\"') && (first_level_tokens[i + 1].Trim()[(first_level_tokens[i + 1].Trim().Length - 1)] == '\"'))
                {
                    tb.Append(first_level_tokens[++i]);
                    i++;
                }
                //if a multiword title enclosed in quotes
                else if (first_level_tokens[i + 1].Trim()[0] == '\"')
                {
                    tb.Append(first_level_tokens[++i] + " ");
                    i++;
                    
                    //forward closure on title string
                    while ((i < first_level_tokens.Length) && (!first_level_tokens[i].Trim().Contains("\"")))
                    {
                        tb.Append(first_level_tokens[i] + " ");
                        i++;
                    }

                    if (i < first_level_tokens.Length)
                        tb.Append(first_level_tokens[i]);
                    else
                        throw new GraphElementFormatException("multiword title or label must be enclosed in quotes: \"  \"");
                }
                else
                {
                    tb.Append(first_level_tokens[++i]);
                    i++;
                }

                title = tb.ToString().Trim();
                if (title.Contains("\""))
                    title = title.Replace("\"", "");
            }
            else
                throw new GraphElementFormatException("bad title label string");

            return title;
        }

        private void OptionsParser(string[] first_level_tokens, ref int i)
        {
            switch (first_level_tokens[i].Trim())
            {
                case "--start":
                case "-s":
                    //start = long.Parse(first_level_tokens[++i].Trim());
                    start = first_level_tokens[++i].Trim();
                    break;
                case "--end":
                case "-e":
                    //this.end = long.Parse(first_level_tokens[++i].Trim());
                    this.end = first_level_tokens[++i].Trim();
                    break;
                case "--units-exponent":
                case "-X":
                    this.units_exponent = int.Parse(first_level_tokens[++i].Trim());
                    break;
                case "--units-length":
                case "-L":
                    this.units_length = int.Parse(first_level_tokens[++i].Trim());
                    break;
                case "--step":
                case "-S":
                    this.step = long.Parse(first_level_tokens[++i].Trim());
                    break;
                case "--upper-limit":
                case "-u":
                    this.yupperlimit = Double.Parse(first_level_tokens[++i].Trim());
                    break;
                case "--lower-limit":
                case "-l":
                    this.ylowerlimit = Double.Parse(first_level_tokens[++i].Trim());
                    break;
                case "--zoom":
                    this.zoom = int.Parse(first_level_tokens[++i].Trim());
                    break;
                case "--title":
                case "-t":
                    this.title = ExtractTitle(first_level_tokens, ref i);
                    break;
                case "--vertical-label":
                case "-v":
                    this.yaxislabel = ExtractTitle(first_level_tokens, ref i);
                    break;
                case "--width":
                case "-w":
                    width = int.Parse(first_level_tokens[++i].Trim());
                    break;
                case "--height":
                case "-h":
                    this.height = int.Parse(first_level_tokens[++i].Trim());
                    break;
                case "--logarithmic":
                case "-o":
                    logarithmic = true;
                    break;
                case "--only-graph":
                case "-j":
                    onlygraph = true;
                    break;
                case "--full-size-mode":
                case "-D":
                    full_size_mode = true;
                    break;
                case "--rigid":
                case "-r":
                    rigid = true;
                    break;
                case "--alt-autoscale":
                case "-A":
                    autoscale = true;
                    break;
                case "--alt-autoscale-min":
                case "-J":
                    autoscale_min = true;
                    break;
                case "--alt-autoscale-max":
                case "-M":
                    autoscale_max = true;
                    break;
                case "--no-gridfit":
                case "-N":
                    no_gridfit = true;
                    break;
                case "--units=si":
                    units_si = true;
                    break;
                case "--alt-y-grid":
                case "-Y":
                    alt_y_grid = true;
                    break;
                case "--x-grid":
                case "-x":
                    {
                        if ((i + 1) < first_level_tokens.Length)
                            if (first_level_tokens[i + 1].ToLower() != "none")
                                _xgrid = new XGrid(first_level_tokens[++i].Split(new char[] { ':' }));
                            else
                            {
                                ++i;
                                _xgrid = new XGrid(XGrid.GTYPE.NONE);
                            }
                    }
                    break;
                case "--y-grid":
                case "-y":
                    {
                        if ((i + 1) < first_level_tokens.Length)
                            if (first_level_tokens[i + 1].ToLower() != "none")
                            {
                                string[] args = first_level_tokens[++i].Split(new char[] { ':' });
                                _ygrid = new YGrid(Double.Parse(args[0]), Double.Parse(args[1]));
                            }
                            else
                            {
                                ++i;
                                _ygrid = new YGrid(YGrid.GTYPE.NONE);
                            }
                    }
                    break;
            }
        }

        private bool ContainsDEF(string def_name)
        {
            foreach (DEF def in _def_list)
                if (def_name == def.name)
                    return true;
            return false;
        }

        private bool ContainsCDEF(string cdef_name)
        {
            foreach (CDEF cdef in _cdef_list)
               if (cdef_name == cdef.vname)
                    return true;
            return false;
        }

       /* private bool ContainsELEM(string  elem_name)
        {
            foreach (IGRAPH_ELEMENT elem in _elem_list)
                if (elem is AGRAPH_ELEMENT)
                {
                    if (elem_name == ((AGRAPH_ELEMENT)elem).val)
                        return true;
                }
                return false;
        } */

        private static List<int> FindGraphElements(string gr)
        {
            int index;
            string[] elem_types = { "LINE", "AREA", "STACK", "HRULE", "VRULE"};
            List<int> index_list = new List<int>();

            //gr = gr.ToUpper();

            for(int i = 0; i < gr.Length; i++)
               for(int j=0; j < elem_types.Length; j++)
                  if((index = gr.IndexOf(elem_types[j], i)) > 0)
                      if(!index_list.Contains(index))
                          index_list.Add(index);
          
            return index_list;
        }

       
        private static bool is_Elem(int index, StringBuilder sb)
        {
            string[] graph_elems = new string[] {"LINE", "AREA", "HRULE", "VRULE"};

            foreach(string elem in graph_elems)
              if((index + elem.Length) < sb.Length)
                 if(sb.ToString().IndexOf(elem, index) == index)
                     return true;

            return false;
        }

        public void PostSerializeFormat()
        {
            foreach (IGRAPH_ELEMENT elem in this._elem_list)
                if (elem is AGRAPH_ELEMENT)
                    ((AGRAPH_ELEMENT)elem).legend = ((AGRAPH_ELEMENT)elem).legend.Replace('', ' ');
        }

        public static string PreSerializeFormat(string gr_str)
        {
            StringBuilder gr = new StringBuilder(gr_str);
            List<int> index_list = FindGraphElements(gr.ToString());


            foreach (int elem_index in index_list)
                for (int i = elem_index; i < gr.Length; i++)
                    if (gr[i] == ' ' && !is_Elem(i + 1, gr))
                        gr[i] = '';

            return gr.ToString();
        }
            
              
      /*  private void RemoveSlashes(String[] slashes)
        {
            for (int i = 0; i < slashes.Length; i++)
                slashes[i] = slashes[i].Replace("\\", "");
        }
        */

#if TEST_GRAPH
        public static void Main()
        {
            //should not need to have this compiled if rrdtool is in the path,
            //if an exception is thrown, just uncomment
            NHawkCommand.Instance.RRDCommandPath = @"e:\downloads\rrdtool\release\rrdtool.exe";

            Console.WriteLine("Testing XGrid class");
            Console.WriteLine("===================");

            Console.WriteLine("\nTesting Protomtion ctor # 1");
            XGrid xgrid = new XGrid(XGrid.TM.MINUTE, 10, XGrid.TM.HOUR, 1, XGrid.TM.HOUR, 4, 0, "%X");
            Console.WriteLine("GTM          : {0}", xgrid.GTM);
            Console.WriteLine("GST          : {0}", xgrid.GST);
            Console.WriteLine("MTM          : {0}", xgrid.MTM);
            Console.WriteLine("MST          : {0}", xgrid.MST);
            Console.WriteLine("LTM          : {0}", xgrid.LTM);
            Console.WriteLine("LST          : {0}", xgrid.LST);
            Console.WriteLine("LPR          : {0}", xgrid.LPR);
            Console.WriteLine("LFM          : {0}", xgrid.LFM);
            Console.WriteLine("Serialization: {0}", xgrid.ToString());
            
            Console.WriteLine("\nTesting DeepClone");
            XGrid xgrid2 = XGrid.DeepClone(xgrid);

            Console.WriteLine("GTM          : {0}", xgrid2.GTM);
            Console.WriteLine("GST          : {0}", xgrid2.GST);
            Console.WriteLine("MTM          : {0}", xgrid2.MTM);
            Console.WriteLine("MST          : {0}", xgrid2.MST);
            Console.WriteLine("LTM          : {0}", xgrid2.LTM);
            Console.WriteLine("LST          : {0}", xgrid2.LST);
            Console.WriteLine("LPR          : {0}", xgrid2.LPR);
            Console.WriteLine("LFM          : {0}", xgrid2.LFM);
            Console.WriteLine("Serialization: {0}", xgrid2.ToString());

            Console.WriteLine("\nTesting Promotion ctor #2 (Deserialization)"); 
            XGrid xgrid3 = new XGrid(xgrid2.ToString().Split(new char[] {':'}));

            Console.WriteLine("GTM          : {0}", xgrid3.GTM);
            Console.WriteLine("GST          : {0}", xgrid3.GST);
            Console.WriteLine("MTM          : {0}", xgrid3.MTM);
            Console.WriteLine("MST          : {0}", xgrid3.MST);
            Console.WriteLine("LTM          : {0}", xgrid3.LTM);
            Console.WriteLine("LST          : {0}", xgrid3.LST);
            Console.WriteLine("LPR          : {0}", xgrid3.LPR);
            Console.WriteLine("LFM          : {0}", xgrid3.LFM);
            Console.WriteLine("Serialization: {0}", xgrid3.ToString());

            Console.WriteLine("Testing YGrid class");
            Console.WriteLine("===================");

            //Console.WriteLine("\nTesting Protomtion ctor # 1");
           // YGrid ygrid = new YGrid(10.5, "this is YGRID label", 10);
           // Console.WriteLine("Serialization: {0}", ygrid.ToString());

            Console.WriteLine("\nTesting graph class");
            Console.WriteLine("===================");
            string start_time = "920804700";
            string end_time = (int.Parse(start_time) + 86400).ToString();  //(1 day time interval)

            RRD rrd1 = RRD.load("random_gen.rrd");
            Console.WriteLine("Loading existing RRD: {0}", rrd1.filename);

          
            Console.WriteLine("\nTesting graph promotion ctor # 1");
            GRAPH gr1 = new GRAPH("graph_test1.png", start_time, end_time);

            gr1.addDEF(new DEF("myrand", rrd1, "rand1000", RRA.CF.AVERAGE));
            gr1.addGELEM(new LINE(2,"myrand", Color.Beige, "test", true));
           
            //gr1.step = 7200;
            Console.WriteLine("filename      : {0}", gr1.filename);
            Console.WriteLine("Start Time    : {0}", gr1.start);
            Console.WriteLine("End Time      : {0}", gr1.end);
            Console.WriteLine("Step          : {0}", gr1.step);
            Console.WriteLine("Title         : {0}", gr1.title);
            Console.WriteLine("Yaxis Label   : {0}", gr1.yaxislabel);
            Console.WriteLine("Width         : {0}", gr1.width);
            Console.WriteLine("Height        : {0}", gr1.height);
            Console.WriteLine("Log Scale     : {0}", gr1.logarithmic);
            Console.WriteLine("XGrid         : {0}", gr1.xgrid_ToString());
            Console.WriteLine("Full size mode: {0}", gr1.full_size_mode);
            Console.WriteLine("Serialization : {0}", gr1.ToString());
            Console.WriteLine("gr1 written as: {0}", gr1.filename);
            gr1.graph();


            //LINE a = new LINE("hssh", null);
            //a.dash_offset = 10;
            //a.set_dashes(true);

            Console.WriteLine("\nTesting graph promotion ctor # 2");
            GRAPH gr2 = new GRAPH("graph_test2.png", start_time, end_time,
                            new DEF("myrand_test", "random_gen.rrd", "rand1000", RRA.CF.AVERAGE, 1, "920804700", "start+1h", RRA.CF.AVERAGE),
                            //new DEF("myrand_test", rrd1, "rand1000", RRA.CF.AVERAGE),
                            new AREA("myrand_test", Color.Blue));

           
            Console.WriteLine("Filename      : {0}", gr2.filename);
            Console.WriteLine("Start Time    : {0}", gr2.start);
            Console.WriteLine("End Time      : {0}", gr2.end);
            Console.WriteLine("Step          : {0}", gr2.step);
            Console.WriteLine("Title         : {0}", gr2.title);
            Console.WriteLine("Yaxis Label   : {0}", gr2.yaxislabel);
            Console.WriteLine("Width         : {0}", gr2.width);
            Console.WriteLine("Height        : {0}", gr2.height);
            Console.WriteLine("Log Scale     : {0}", gr2.logarithmic.ToString());
            Console.WriteLine("XGrid         : {0}", gr2.xgrid_ToString());
            Console.WriteLine("Full size mode: {0}", gr2.full_size_mode);
            Console.WriteLine("Serialization : {0}", gr2.ToString());
            Console.WriteLine("gr2 written as: {0}", gr2.filename);
            gr2.graph();


            Console.WriteLine("\nTesting graph promotion ctor # 3");
            GRAPH gr3 = new GRAPH("graph_test3.png", start_time, end_time);
        
            gr3.bind(new DEF("myrand_10000", rrd1, rrd1.ds_lookup(1).name, RRA.CF.AVERAGE),
                         new LINE(2,"myrand_10000", Color.Green,"random numbers between 2000 and 3000",false));
            gr3.bind(new DEF("myrand_1000", rrd1, rrd1.ds_lookup(0).name, RRA.CF.AVERAGE),
                         new LINE(2, "myrand_1000", Color.Red, "random numbers betweem 0 and 1000", false));

            gr3.addGELEM(new SHIFT("myrand_1000", 10));

            gr3.title = "Testing .Net Random number generator for uniform generation";
            gr3.yaxislabel = "Random";

            gr3.xgrid(XGrid.TM.MINUTE, 10, XGrid.TM.HOUR, 1, XGrid.TM.HOUR, 4, 0, "%X");
            gr3.ygrid(10.5, 10.5);
            gr3.units_length = 5;
            gr3.units_si = true;
            //gr3.units_exponent = 0;
            gr3.zoom = 1;

            //gr3.ygrid(YGrid.GTYPE.NONE);
            //gr3.ygrid(YGrid.GTYPE.AUTO);

            //gr3.xgrid = new XGrid(XGrid.TM.MINUTE, 10, XGrid.TM.HOUR, 1, XGrid.TM.HOUR, 4, 0, "%X");
            Console.WriteLine("Hashcode      : {0}", gr3.GetHashCode());
            Console.WriteLine("Filename      : {0}", gr3.filename);
            Console.WriteLine("Start Time    : {0}", gr3.start);
            Console.WriteLine("End Time      : {0}", gr3.end);
            Console.WriteLine("Step          : {0}", gr3.step);
            Console.WriteLine("Title         : {0}", gr3.title);
            Console.WriteLine("Yaxis Label   : {0}", gr3.yaxislabel);
            Console.WriteLine("Width         : {0}", gr3.width);
            Console.WriteLine("Height        : {0}", gr3.height);
            Console.WriteLine("Log Scale     : {0}", gr3.logarithmic.ToString());
            Console.WriteLine("XGrid         : {0}", gr3.xgrid_ToString());
            Console.WriteLine("YGrid         : {0}", gr3.ygrid_ToString());
            Console.WriteLine("Full size mode: {0}", gr3.full_size_mode);
            Console.WriteLine("Serialization : {0}", gr3.ToString());
            Console.WriteLine("gr3 written as: {0}", gr3.filename);
 
            gr3.graph("e:\\");
          
           Console.WriteLine("\nTesting DeepClone");
           GRAPH gr4 = GRAPH.DeepClone(gr3);
          

            gr4.filename = "graph_test4.png";
            Console.WriteLine("\nHashcode      : {0}", gr4.GetHashCode());
            Console.WriteLine("Filename      : {0}", gr4.filename);
            Console.WriteLine("Start Time    : {0}", gr4.start);
            Console.WriteLine("End Time      : {0}", gr4.end);
            Console.WriteLine("Step          : {0}", gr4.step);
            Console.WriteLine("Title         : {0}", gr4.title);
            Console.WriteLine("Yaxis Label   : {0}", gr4.yaxislabel);
            Console.WriteLine("Width         : {0}", gr4.width);
            Console.WriteLine("Height        : {0}", gr4.height);
            Console.WriteLine("Log Scale     : {0}", gr4.logarithmic.ToString());
            Console.WriteLine("XGrid         : {0}", gr4.xgrid_ToString());
            Console.WriteLine("YGrid         : {0}", gr4.ygrid_ToString());
            Console.WriteLine("Full size mode: {0}", gr4.full_size_mode);
            Console.WriteLine("Serialization : {0}", gr4.ToString());
            Console.WriteLine("gr4 written as: {0}", gr4.filename);
            gr4.graph("e:\\");

            
            Console.WriteLine("\nTest Deserialization");
            GRAPH gr5 = new GRAPH(gr4.ToString(), "e:\\");
            gr5.full_size_mode = true;
           
            //gr5.xgrid = null;
            Console.WriteLine("Hashcode      : {0}", gr5.GetHashCode());
            Console.WriteLine("Filename      : {0}", gr5.filename);
            Console.WriteLine("Start Time    : {0}", gr5.start);
            Console.WriteLine("End Time      : {0}", gr5.end);
            Console.WriteLine("Step          : {0}", gr5.step);
            Console.WriteLine("Title         : {0}", gr5.title);
            Console.WriteLine("Yaxis Label   : {0}", gr5.yaxislabel);
            Console.WriteLine("Width         : {0}", gr5.width);
            Console.WriteLine("Height        : {0}", gr5.height);
            Console.WriteLine("Log Scale     : {0}", gr5.logarithmic.ToString());
            Console.WriteLine("XGrid         : {0}", gr5.xgrid_ToString());
            Console.WriteLine("YGrid         : {0}", gr5.ygrid_ToString());
            Console.WriteLine("Full size mode: {0}", gr5.full_size_mode);
            Console.WriteLine("Serialization : {0}", gr5.ToString());
            Console.WriteLine("gr3 written as: {0}", gr5.filename);
            
            
            /*
            Console.WriteLine("\nTesting Accessor / Mutator Properties");
            
            gr5.filename = "test_gr5_graph.png";
            //gr5.step = 300;
            //gr5.logarithmic = true;
            //gr5.width = gr5.height = 200;
            //gr5.title = "some title";
            //gr5.yaxislabel = "some y title";
            //gr5.ylowerlimit = -2.4;
            //gr5.yupperlimit = 100.5;
            // gr5.rigid = true;
            // gr5.onlygraph = true;
            //gr5.autoscale = true;
            //gr5.autoscale_min = true;
            //gr5.autoscale_max = true;
            gr5.no_gridfit = true;

            gr5.graph("e:\\");

            
            Console.WriteLine("Hashcode      : {0}", gr5.GetHashCode());
            Console.WriteLine("Filename      : {0}", gr5.filename);
            Console.WriteLine("Start Time    : {0}", gr5.start);
            Console.WriteLine("End Time      : {0}", gr5.end);
            Console.WriteLine("Step          : {0}", gr5.step);
            Console.WriteLine("Title         : {0}", gr5.title);
            Console.WriteLine("Yaxis Label   : {0}", gr5.yaxislabel);
            Console.WriteLine("Width         : {0}", gr5.width);
            Console.WriteLine("Height        : {0}", gr5.height);
            Console.WriteLine("Log Scale     : {0}", gr5.logarithmic.ToString());
            Console.WriteLine("y lower limit : {0}", gr5.ylowerlimit);
            Console.WriteLine("y upper limit : {0}", gr5.yupperlimit);
            Console.WriteLine("is rigid set  : {0}", gr5.rigid.ToString());
            Console.WriteLine("is graph only : {0}", gr5.onlygraph.ToString());
            Console.WriteLine("Serialization : {0}", gr5.ToString());
            Console.WriteLine("gr5 written as: {0}", gr5.filename);

           
            Console.WriteLine("Test Deserialization again (with mutator options) ");
            GRAPH gr6 = new GRAPH(gr5.ToString(), "e:\\");
            Console.WriteLine("Filename      : {0}", gr6.filename);
            Console.WriteLine("Start Time    : {0}", gr6.start);
            Console.WriteLine("End Time      : {0}", gr6.end);
            Console.WriteLine("Step          : {0}", gr6.step);
            Console.WriteLine("Title         : {0}", gr6.title);
            Console.WriteLine("Yaxis Label   : {0}", gr6.yaxislabel);
            Console.WriteLine("Width         : {0}", gr6.width);
            Console.WriteLine("Height        : {0}", gr6.height);
            Console.WriteLine("Log Scale     : {0}", gr6.logarithmic.ToString());
            Console.WriteLine("y lower limit : {0}", gr6.ylowerlimit);
            Console.WriteLine("y upper limit : {0}", gr6.yupperlimit);
            Console.WriteLine("is rigid set  : {0}", gr6.rigid.ToString());
            Console.WriteLine("is graph only : {0}", gr6.onlygraph.ToString());
            Console.WriteLine("Serialization : {0}", gr6.ToString());
            Console.WriteLine("gr6 written as: {0}", gr6.filename);
            
            */            
            
            /* still need to test exception handling */
        }
#endif 

    }
}
