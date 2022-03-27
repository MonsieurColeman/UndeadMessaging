/*
 This file contains the strings of the client side application
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanPeerToPeer
{
    public static class GlobalStrings
    {
        public static readonly string inputValidation_Login = "Notice! \n\nYour username may only contain:" +
                    "\n > Letters \n > Numbers \n > Underscores \n > Dashes \n > 15 characters";
        public static readonly string popup_duplicateUsername = "That name currently is in use!";
        public static readonly string inputValidation_TopicCreation = "Notice: Your topic name can only contain characters!";
        public static readonly string tag_TopicCreation = "Topic: ";
        public static readonly string error_unexpectedMsgType = "I receive a weird message";
        public static readonly string lipsum_Image = "https://picsum.photos/200/300";
        public static readonly string color_black = "#000000";
    }
}

/*
 1.0 File create and a mass dump of the application's strings in the name of order
 */