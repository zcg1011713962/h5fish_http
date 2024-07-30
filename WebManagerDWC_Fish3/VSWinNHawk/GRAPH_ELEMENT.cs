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
//File       : GRAPH_ELEMENT.cs   -- .Net / Mono graph element common base class                              //
//Application: NHawk: Open Source Project Support                                                             // 
//Language   : C# .Net 3.5, Visual Studio 2008                                                                //
//Author     : Mike Corley, Syracuse University                                                               //
//             mwcorley@syr.edu                                                                               //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/* Module Operations
 * =================
 * This module provides a common abstract base class for deriving valid (rrdtool) graph elements such as LINE,
 * AREA, HRULE, VRULE, etc. The GRAPH_ELEMENT class provides one abstract method: "DeepClone()" and one virtual method "ToString()".
 * The model is that all graph elements share a common feature set, and all NHAWK constructs are represented internally as composite 
 * structures so each specific element must know how to clone (DeepClone), serialize (ToString), and deserialize itself.  
 * Deserialization is supported through the derived class constructor so it doesn't appear as part of the base class interface.  
 * 
 * Note about this class and the NHAWK project: This class seeks to encapsulate rrdtool constructs the in the C# .Net / Mono 
 * object model while retaining intuitive use of the RRDTool syntax. It seeks to enforce correct usage of rrdtool
 * constructs by providing semantically appropriate exception handling.  The primary goal of the NHAWK project is two fold. One to enable
 * .Net developers to use RRDTool directly from C#, while retaining the native (intuitive use) of rrdtool construct sytnax.  
 * .i.e. to constitute a very thin rrdtool provider layer enabling proficient .Net developers to rapidly gain productivity by referring 
 * directly and intuitively to the main rrdtool documentation page at: http://oss.oetiker.ch/rrdtool/doc/rrdgraph.en.html. Secondly to
 * liberate the full potential of rrdtool within the rich .Net 2.0/3.0/3.5 framework.
 * 
 * 
 * Public Interface
 * ================  
 * 
 *   [Serializable]
 *    public class GraphElementFormatException
 *      -- exception class thrown by GRAPH_ELEMENT and derived class instances
 *         for enforcing proper construct usage
 *         
 *   public interface IGRAPH_ELEMENT
 *    {
 *       IGRAPH_ELEMENT DeepClone();
 *       string ToString();
 *    }
 * 
 *   [Serializable]
 *   public abstract class AGRAPH_ELEMENT : IGRAPH_ELEMENT
 *   
 *   protected AGRAPH_ELEMENT()  
 *      -- cconstructor: ensures all member state initialization
 *      
 *   protected AGRAPH_ELEMENT(TYPE t, string val_name)    
 *      -- constructor:  accepts type and val_name (the basic def). e.g [LINE][AREA]:1000
 *      
 *   protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c)
 *      -- constructor:  accepts type and val_name, and color e.g [LINE][AREA]:1000#color
 *      
 *   protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c, string legend, bool stack)
 *      -- constructor:  accepts type and val_name, and color e.g [LINE][AREA]:1000#color:"this a a legend"
 *                       note: a GraphFormatException will be thrown if legend is specified while color == Color.empty
 *                       
 *   protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c, string legend, bool stack, bool dash_val)
 *      -- constructor:  accepts type and val_name, color, legend, stack and line dash . 
 *                       e.g LINE:1000#color:"this a a legend":STACK:dashes
 *      -- note: dashes field has no effect for non line oriented elements such as AREA, and therefore should be false
 *      
 *   protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c, string legend, bool stack, int[] dash_vals)
 *    -- constructor:  accepts type and val_name, color, legend, stack and line dash values 
 *                       e.g LINE:1000#color:"this a a legend":STACK:dashes=10,5,5,10
 *    -- note: dash_vals field has no effect for non line oriented elements such as AREA, and there this ctor is not 
 *             appropriate for non line oriented elements. e.g. AREA
 *      
 *   protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c, string legend, bool stack, int[] dash_vals, int dash_offset) 
 *   -- constructor:  accepts type and val_name, color, legend, stack and line dash values 
 *                    e.g LINE:1000#color:"this a a legend":STACK:dashes=10,5,5,10:dash-offset=10
 *   -- note: dash_vals and dash_offset field has no effect for non line oriented elements such as AREA, and there this 
 *            ctor is not appropriate for non line oriented elements. e.g. AREA
 *   
 *   public abstract IGRAPH_ELEMENT DeepClone()
 *   -- derived class must implement 
 *      LINE, AREA, HRULE, etc. all must specify how to clone themselves.
 *      note: this is a deep clone and that doesns't generally fit, 
 *      the shallow reference model, but it works well here because all
 *      of NHAWK base/derived class member state is composed of value types
 *      and string references.  value types get deep copied and new memory instances
 *      of strings are returned with careful StringBuilder usage.  See my discussion on
 *      shallow reference versus deep copy semantics on the main page
 *        
 *   public void set_dashes(bool dashes_val)
 *   -- sets the dashes option to true or false.  Has NO effect when applied to AREA elements
 *      e.g.  (TRUE)  LINE:1000#color:"this a a legend":dashes
 *            (FALSE) LINE:1000#color:"this a a legend"
 *       
 *   public void set_dashes(int[] dashes_args)
 *   -- sets the dashes option values. "set_dashes(false)" removes this option. 
 *      Has NO effect when applied to AREA elements
 *      e.g.  LINE:1000#color:"this a a legend":dashes=10,5,5,10
 *                 
 *   public int dash_offset
 *   -- gets or sets the dash-offset value. set == -1 when no dash-offset is desired
 *      If applied when dases == false, and GraphElementFormatException will be thrown
 *      Has no effect when applied to AREA elements
 *      e.g.  LINE:1000#color:"this a a legend":dashes:dash-offset=10
 *      
 *   public TYPE type
 *   -- gets or the element type: (AREA, LINE)
 *    
 *   public Color color
 *   -- gets or sets the color of the visualizable element
 *      e.g. [LINE][AREA]:1000#color:"this a a legend"
 *    
 *   public string val
 *   -- gets or sets the graph definition variable or value.  
 *      val be consist of a  contiguous sequence of characters (no spaces) 
 *      or GraphElementFormatException will be thrown.
 *      [LINE][AREA]:val
 *   
 *   public bool stack
 *   -- gets or sets a boolean which determines whether or not this element is stacked above
 *      the previously defined graph element
 *      [LINE][AREA]:1000#color:"this a a legend"
 *         or
 *      [LINE][AREA]:1000#color:"this a a legend":STACK
 *      
 *   public string legend
 *      --gets or sets the legend. Note:  if specified when color == Color.IsEmpty == true, and
 *        a GraphFormatException will be thrown.
 *     [LINE][AREA]:1000#color:"this a a legend"  
 * 
 *   protected string dashes
 *   -- used by the derived class for serialization/and deserialization of "dashes" options.
 *      NOT subject to direct client invocation.
 *      
 *   public override string ToString()
 *   -- must be overridden by derived class for proper serialization semantics 
 *   
 * Example Usage
 * =============
 * See a derived class test stub. AREA, LINE, etc.
 * 
 * Build Process
 * =============
 * This module is not intended for stand alone for testing purposes 
 * See a derived class: AREA, LINE, VRULE, HRULE, etc.
 * 
 * Maintanence History
 * ===================
 * version 1.0 : 01 August 08 
 *     -- first release: 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NHAWK
{
    [Serializable]
    public class GraphElementFormatException : Exception
    {
        public GraphElementFormatException(string message)
            : base(message)
        { }

        public GraphElementFormatException(string message, Exception ex)
            : base(message, ex)
        { }
    };

    
    public interface IGRAPH_ELEMENT
    {
        IGRAPH_ELEMENT DeepClone();
        string ToString();
    }


    [Serializable]
    public abstract class AGRAPH_ELEMENT : IGRAPH_ELEMENT
    {
        public enum TYPE
        {
            LINE, AREA, STACK, HRULE, VRULE
        };

        private Color  _color;
        private string _value;
        private TYPE   _type;
        private string _legend;
        private bool   _stack;
        private string _dashes;
        private int    _dash_offset;
        protected const int MIN_TOKEN_COUNT = 2;

     
        //default ctor -  ensures proper initialization
        protected AGRAPH_ELEMENT()
        {
            //myset_type = TYPE.UNKWOWN;
            legend = "";
            color = Color.Empty;
            val = "";
            stack = false;
            set_dashes(false);
            dash_offset = -1;
           
        }

        protected AGRAPH_ELEMENT(TYPE t, string val_name): this()
        {
            myset_type = t;
            val = val_name;
        }
        
        protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c)
            : this(t, val_name)
        {
            //This doesn't work for me! 
            //_color = System.Drawing.ColorTranslator.ToHtml(c);
            color = c;
        }

        protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c, string legend, bool stack)
            : this(t, val_name, c)
        {
            this.legend = legend;
            this.stack = stack;
        }

        protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c, string legend, bool stack, bool dash_val)
            : this(t, val_name, c, legend, stack)
        {
            set_dashes(dash_val);
        }

        protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c, string legend, bool stack, int[] dash_vals) 
            : this(t,val_name, c, legend, stack)
        {
            set_dashes(dash_vals);
        }

        protected AGRAPH_ELEMENT(TYPE t, string val_name, Color c, string legend, bool stack, int[] dash_vals, int dash_offset)
            : this(t, val_name, c, legend, stack, dash_vals)
        {
            this.dash_offset = dash_offset;
        }


     
        /* public override string ToString()
        {
            return "GRAPH_ELEMENT_BASE";
        } */

        public abstract IGRAPH_ELEMENT DeepClone();
        

        public void set_dashes(bool dashes_val)
        {
            dashes = (dashes_val) ? "dashes" : "";
        }

        public void set_dashes(int[] dashes_args)
        {
            StringBuilder d_builder = new StringBuilder();

            d_builder.Append(dashes_args[0]);

            for(int i = 1; i < dashes_args.Length; i++)
                d_builder.Append("," + dashes_args[i]);

            dashes = d_builder.ToString();
        }

        public int dash_offset
        {
            get { return _dash_offset; }
            set
            {
                //may not be good to explicity refer to AREA type
                //here but will have the desired effect without
                //limiting functionality
                if (ELMtype != TYPE.AREA)
                {
                    if (value != -1 && dashes == "")
                        throw new GraphElementFormatException("dashes-offset option must be follow \"dashes\"  use dashes=true");

                    _dash_offset = value;
                }
            }
        }

        public TYPE ELMtype
        {
            get { return _type; }
            // set { _type = value; }
        }
        
        public Color color
        {
            get { return _color; }
            set { _color = value; }
        }

        public string val
        {
            get { return _value; }
            set
            {
                if (check_format(value))
                    _value = value;
                else
                    throw new GraphElementFormatException("element value cannot have spaces");
            }
        }

        public bool stack
        {
            get { return _stack; }
            set
            {
                _stack = value;
            }
        }

        public string legend
        {
            get { return _legend; }
            set
            {

                if (color.IsEmpty && value != "")
                    throw new GraphElementFormatException("color must be specified to use a legend");

                _legend = value.Trim();

                if (_legend != "")
                {
                    if (_legend[0] != '\"')
                        _legend = "\"" + _legend;

                    if (legend[legend.Length - 1] != '\"')
                        _legend = _legend + "\"";
                }
                
            }
        }


        protected string dashes
        {
            get { return _dashes; }
            set { _dashes = value; }
        }


        private int get_legend_stop_token(string[] elem_tokens, int start_token)
        {
            int i; //= start_token;
            for (i = start_token; i < elem_tokens.Length; i++)
                if ((elem_tokens[i].Trim().ToLower() == "stack") ||
                    (elem_tokens[i].Trim().ToLower().Contains("dashes")))
                    return i;

            return i;
        }

        
        private void extract_dashes_option(string dashes_token)
        {
           string[] dashes_option = dashes_token.Split(new char[] {'='});

           if (dashes_option.Length == 2)
               dashes = dashes_option[1];
           else if (dashes_option.Length == 1 && dashes_token == "dashes")
               set_dashes(true);
           else
               throw new GraphElementFormatException("bad LINE element format... expected \"dashes\" option?");
        } 
        

        private int extract_dash_offset_option(string dash_offset_token)
        {
            string[] dash_offset_args = dash_offset_token.Split(new char[] { '=' });

            if (dash_offset_args.Length == 2)
                return int.Parse(dash_offset_args[1]);
            else
                throw new GraphElementFormatException("Expected \"dash-offset\": value?");
        }

        protected void token_closure(string[] elem_tokens)
        {
            int start_token = MIN_TOKEN_COUNT;
            int stop_token = get_legend_stop_token(elem_tokens, start_token);

            //stop > start then a legend must exist so collect it
            if (stop_token > start_token)
            {
                StringBuilder legend_builder = new StringBuilder();
                legend_builder.Append(elem_tokens[start_token]);

                //need to collect any other tokens that might have an ':' as part of the legend itself
                for (int i = start_token + 1; i < stop_token; i++)
                    legend_builder.Append(":" + elem_tokens[i]);

                legend = legend_builder.ToString();
            }

            //collect any other token attributes
            for (int i = stop_token; i < elem_tokens.Length; i++)
            {
                string tok = elem_tokens[i].Trim().ToLower();

                if (tok == "stack")
                    stack = true;
                else if (tok.Contains("dashes"))
                    extract_dashes_option(elem_tokens[i]);
                else
                    if (tok.Contains("dash-offset"))
                        this.dash_offset = extract_dash_offset_option(elem_tokens[i]);

            };
        }


        protected TYPE myset_type
        {
            get { return _type; }
            set { _type = value; }
        }


        protected string ElemToStr(TYPE type)
        {
            switch (type)
            {
                case AGRAPH_ELEMENT.TYPE.LINE:
                    return "LINE";
                case AGRAPH_ELEMENT.TYPE.AREA:
                    return "AREA";
                case AGRAPH_ELEMENT.TYPE.HRULE:
                    return "HRULE";
                case AGRAPH_ELEMENT.TYPE.VRULE:
                    return "VRULE";
                default:
                    return "STACK";
            };
        }

        protected TYPE StrToElem(string type)
        {
            switch (type)
            {
                case "LINE":
                    return AGRAPH_ELEMENT.TYPE.LINE;
                case "AREA":
                    return AGRAPH_ELEMENT.TYPE.AREA;
                case "STACK":
                    return AGRAPH_ELEMENT.TYPE.STACK;
                case "HRULE":
                    return AGRAPH_ELEMENT.TYPE.HRULE;
                case "VRULE":
                    return AGRAPH_ELEMENT.TYPE.VRULE;
                default:
                    throw new GraphElementFormatException("Unknown Element Type: " + type);
            };
        }
     
        protected string ColorToHex(Color c)
        {
            string R = Microsoft.VisualBasic.Conversion.Hex(c.R);
            string G = Microsoft.VisualBasic.Conversion.Hex(c.G);
            string B = Microsoft.VisualBasic.Conversion.Hex(c.B);

            if (R.Length == 1) R = "0" + R;
            if (G.Length == 1) G = "0" + G;
            if (B.Length == 1) B = "0" + B;

            return "#" + (R + G + B);
        }

        protected bool check_format(string val)
        {
            foreach (char ch in val)
                if (ch == ' ' || ch == '\t')
                    return false;

            return true;
        }
    }
}