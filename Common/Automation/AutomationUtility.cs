using Common.AutomationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Automation
{
    public class AutomationUtility
    {
        public static int SendAutomationMessage(string _Subject, string _MSG, string[] _MainPersonalID, string[] _CopyPersonID)
        {
            int SuccessSendCount = 0;
            try
            {
                // send AutoMsg with سیستم مدیریت کیفی خودرو
                PostItemService ps = new PostItemService();
                //ipid[0].
                var postItem = new PostItem
                {
                    Subject = _Subject,
                    Description = _MSG,
                    ImportanceType = ImportanceType.Normal
                };
                string Pid = "";
                Participant participant;
                SendPostItemType returnType;
                InvolvedPerson[] lstP = new InvolvedPerson[_MainPersonalID.Length + _CopyPersonID.Length];
                int PIndex = 0;
                for (int i = 0; i < _MainPersonalID.Length; i++)
                {

                    Pid = _MainPersonalID[i];
                    participant = ps.FindPersonalNo("product", "percamp0082397171", Pid);
                    if (participant != null)
                    {
                        lstP[PIndex++] = new InvolvedPerson
                        {
                            InvolvedParticipantID = participant.Id,
                            InvolvedType = InvolvedType.MainReceiver
                        };
                    }
                }
                for (int j = 0; j < _CopyPersonID.Length; j++)
                {

                    Pid = _CopyPersonID[j];
                    participant = ps.FindPersonalNo("product", "percamp0082397171", Pid);
                    if (participant != null)
                    {
                        lstP[PIndex++] = new InvolvedPerson
                        {
                            InvolvedParticipantID = participant.Id,
                            InvolvedType = InvolvedType.CopyReceiver
                        };
                    }
                }
                //---
                postItem.InvolvedPersonList = lstP;
                returnType = ps.SendPostItem("product", "percamp0082397171", postItem);
                if (returnType == SendPostItemType.SendSuccessful)
                    SuccessSendCount++;

                return SuccessSendCount;
            }
            catch
            {
                return SuccessSendCount;
            }
        }


        public static int SendAutomationMessageWithAttachment(string _Subject, string _MSG, string[] _MainPersonalID, string[] _CopyPersonID, String _FileName, string _FileExtention, byte[] ExcelAttachment)
        {
            int SuccessSendCount = 0;
            try
            {
                // send AutoMsg with سیستم مدیریت کیفی خودرو
                PostItemService ps = new PostItemService();
                //ipid[0].
                var postItem = new PostItem
                {
                    Subject = _Subject,
                    Description = _MSG,
                    ImportanceType = ImportanceType.Normal
                };
                string Pid = "";
                Participant participant;
                SendPostItemType returnType;
                InvolvedPerson[] lstP = new InvolvedPerson[_MainPersonalID.Length + _CopyPersonID.Length];
                int PIndex = 0;
                for (int i = 0; i < _MainPersonalID.Length; i++)
                {

                    Pid = _MainPersonalID[i];
                    participant = ps.FindPersonalNo("product", "percamp0082397171", Pid);
                    if (participant != null)
                    {
                        lstP[PIndex++] = new InvolvedPerson
                        {
                            InvolvedParticipantID = participant.Id,
                            InvolvedType = InvolvedType.MainReceiver
                        };
                    }
                }
                for (int j = 0; j < _CopyPersonID.Length; j++)
                {

                    Pid = _CopyPersonID[j];
                    participant = ps.FindPersonalNo("product", "percamp0082397171", Pid);
                    if (participant != null)
                    {
                        lstP[PIndex++] = new InvolvedPerson
                        {
                            InvolvedParticipantID = participant.Id,
                            InvolvedType = InvolvedType.CopyReceiver
                        };
                    }
                }
                //---
                postItem.InvolvedPersonList = lstP;
                Attachment att = new Attachment();
                att.FileName = _FileName;
                att.FileExtention = _FileExtention;
                att.FileContent = ExcelAttachment;
                Attachment[] at = new Attachment[1];
                at[0] = att;
                postItem.AttachmentList = at;
                returnType = ps.SendPostItem("product", "percamp0082397171", postItem);
                if (returnType == SendPostItemType.SendSuccessful)
                    SuccessSendCount++;

                return SuccessSendCount;
            }
            catch
            {
                return SuccessSendCount;
            }
        }




    }
}
