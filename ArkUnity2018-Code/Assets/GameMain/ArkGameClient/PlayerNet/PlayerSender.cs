using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Google.Protobuf;
using AFTCPClient;
using AFMsg;
using AFCoreEx;

namespace PlayerNetClient
{

public class PlayerSender
{
    PlayerNet mxPlayerNet = null;

    public PlayerSender(PlayerNet clientnet)
    {
        mxPlayerNet = clientnet;
    }

    static public AFMsg.PBGUID AFToPB(AFCoreEx.AFIDENTID xID)
    {
        AFMsg.PBGUID xIdent = new AFMsg.PBGUID();
        xIdent.High = xID.nHead64;
        xIdent.Low = xID.nData64;

        return xIdent;
    }

    private void SendMsg(AFCoreEx.AFIDENTID xID, AFMsg.EGameMsgID unMsgID, MemoryStream stream)
    {
        MsgHead head = new MsgHead();
        head.unMsgID = (UInt16)unMsgID;
        head.nHead64 = xID.nHead64;
        head.nData64 = xID.nData64;
        mxPlayerNet.mxNet.SendMsg(head, stream.ToArray());
    }

    public void SendMsg(AFCoreEx.AFIDENTID xID, AFMsg.EGameMsgID unMsgID, IMessage xData)
    {
        MemoryStream stream = new MemoryStream();
        MessageExtensions.WriteTo(xData, stream);
        SendMsg(xID, unMsgID, stream);
    }

    public void LoginPB(string strAccount, string strPassword, string strSessionKey)
    {
        if(mxPlayerNet.mbDebugMode)
        {
            mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_WAIT_SELECT_ROLE);
            //AFCRenderInterface.Instance.LoadScene("SelectScene");
        }
        else
        {
            AFMsg.ReqAccountLogin xData = new AFMsg.ReqAccountLogin();
            xData.Account = strAccount;
            xData.Password = strPassword;
            xData.SecurityCode = strSessionKey;
            xData.SignBuff = "";
            xData.ClientVersion = 1;
            xData.LoginMode = 0;
            xData.ClientIP = 0;
            xData.ClientMAC = 0;
            xData.DeviceInfo = "";
            xData.ExtraInfo = "";

            SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqLogin, xData);
        }
    }

    public void RequireWorldList()
    {
        AFMsg.ReqServerList xData = new AFMsg.ReqServerList();
        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqWorldList, xData);
    }

    public void RequireConnectWorld(int nWorldID)
    {
        AFMsg.ReqConnectWorld xData = new AFMsg.ReqConnectWorld();
        xData.WorldId = nWorldID;
            
        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqConnectWorld, xData);
    }

    public void RequireVerifyWorldKey(string strAccount, string strKey)
    {
        AFMsg.ReqAccountLogin xData = new AFMsg.ReqAccountLogin();
        xData.Account = strAccount;
        xData.Password = "";
        xData.SecurityCode = strKey;
        xData.SignBuff = "";
        xData.ClientVersion = 1;
        xData.LoginMode = 0;
        xData.ClientIP = 0;
        xData.ClientMAC = 0;
        xData.DeviceInfo = "";
        xData.ExtraInfo = "";

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqConnectKey, xData);
    }

    public void RequireServerList()
    {
        AFMsg.ReqServerList xData = new AFMsg.ReqServerList();
        xData.Type = AFMsg.ReqServerListType.RsltGamesErver;
        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqWorldList, xData);
    }

    public void RequireSelectServer(int nServerID)
    {
        AFMsg.ReqSelectServer xData = new AFMsg.ReqSelectServer();
        xData.WorldId = nServerID;

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqSelectServer, xData);
    }

    public void RequireRoleList(string strAccount, int nGameID)
    {
        AFMsg.ReqRoleList xData = new AFMsg.ReqRoleList();
        xData.GameId = nGameID;
        xData.Account = strAccount;
        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqRoleList, xData);
    }

    public void RequireCreateRole(string strAccount, string strRoleName, int byCareer, int bySex, int nGameID)
    {
        if(strRoleName.Length >= 20 || strRoleName.Length < 1)
        {
            return;
        }

        AFMsg.ReqCreateRole xData = new AFMsg.ReqCreateRole();
        xData.Career = byCareer;
        xData.Sex = bySex;
        xData.NoobName = strRoleName;
        xData.Account = strAccount;
        xData.Race = 0;
        xData.GameId = nGameID;
        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqCreateRole, xData);
    }

    public void RequireDelRole(AFCoreEx.AFIDENTID objectID, string strAccount, string strRoleName, int nGameID)
    {
        AFMsg.ReqDeleteRole xData = new AFMsg.ReqDeleteRole();
        xData.Name = strRoleName;
        xData.Account = strAccount;
        xData.GameId = nGameID;

        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqDeleteRole, xData);
    }

    public void RequireEnterGameServer(AFCoreEx.AFIDENTID objectID, string strAccount, string strRoleName, int nServerID)
    {
        AFMsg.ReqEnterGameServer xData = new AFMsg.ReqEnterGameServer();
        xData.Name = strRoleName;
        xData.Account = strAccount;
        xData.GameId = nServerID;
        xData.Id = AFToPB(objectID);
        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqEnterGame, xData);
    }

    public void RequireHeartBeat(AFCoreEx.AFIDENTID objectID)
    {
        AFMsg.ReqHeartBeat xData = new AFMsg.ReqHeartBeat();
        SendMsg(objectID, AFMsg.EGameMsgID.EgmiStsHeartBeat, xData);
    }

    //有可能是他副本的NPC移动,因此增加64对象ID
    public void RequireMove(AFCoreEx.AFIDENTID objectID, float fX, float fZ)
    {
        AFMsg.ReqAckPlayerMove xData = new AFMsg.ReqAckPlayerMove();
        xData.Mover = AFToPB(objectID);
        xData.MoveType= 0;

        AFMsg.Position xTargetPos = new AFMsg.Position();
        xTargetPos.X = fX;
        xTargetPos.Z = fZ;
        xData.TargetPos.Add(xTargetPos);

        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqMove, xData);
    }

    public void RequireMoveImmune(AFCoreEx.AFIDENTID objectID, float fX, float fZ)
    {
        AFMsg.ReqAckPlayerMove xData = new AFMsg.ReqAckPlayerMove();
        xData.Mover = AFToPB(objectID);
        xData.MoveType = 0;
        AFMsg.Position xTargetPos = new AFMsg.Position();
        xTargetPos.X = fX;
        xTargetPos.Z = fZ;
        xData.TargetPos.Add(xTargetPos);

        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqMoveImmune, xData);
    }

    //有可能是他副本的NPC移动,因此增加64对象ID
    public void RequireUseSkill(AFCoreEx.AFIDENTID objectID, string strKillID, AFCoreEx.AFIDENTID nTargetID, float fNowX, float fNowZ, float fTarX, float fTarZ)
    {
        //Debug.Log("RequireUseSkill:" + strKillID);

        AFMsg.Position xNowPos = new AFMsg.Position();
        AFMsg.Position xTarPos = new AFMsg.Position();

        xNowPos.X = fNowX;
        xNowPos.Y = 0.0f;
        xNowPos.Z = fNowZ;
        xTarPos.X = fTarX;
        xTarPos.Y = 0.0f;
        xTarPos.Z = fTarZ;

        AFMsg.ReqAckUseSkill xData = new AFMsg.ReqAckUseSkill();
        xData.User = AFToPB(objectID);
        xData.SkillId = strKillID;
        xData.TarPos = xTarPos;
        xData.NowPos = xNowPos;

        if(!nTargetID.IsNull())
        {
            AFMsg.EffectData xEffectData = new AFMsg.EffectData();

            xEffectData.EffectIdent = AFToPB(nTargetID);
            xEffectData.EffectValue = 0;
            xEffectData.EffectRlt = 0;
            xData.EffectData.Add(xEffectData);
        }

        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqSkillObjectx, xData);
    }

    public void RequireUseItem(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID nGuid, AFCoreEx.AFIDENTID nTargetID)
    {
        AFMsg.ReqAckUseItem xData = new AFMsg.ReqAckUseItem();
        xData.ItemGuid = AFToPB(nGuid);

        AFMsg.EffectData xEffectData = new AFMsg.EffectData();

        xEffectData.EffectIdent = AFToPB(nTargetID);
        xEffectData.EffectRlt = 0;
        xEffectData.EffectValue = 0;

        xData.EffectData.Add(xEffectData);

        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqItemObject, xData);
    }

    public void RequireChat(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID targetID, int nType, string strData)
    {
        AFMsg.ReqAckPlayerChat xData = new AFMsg.ReqAckPlayerChat();
        xData.ChatId = AFToPB(targetID);
        xData.ChatType = (AFMsg.ReqAckPlayerChat.Types.EGameChatType)nType;
        xData.ChatInfo = strData;

        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqChat, xData);

    }

    public void RequireSwapScene(AFCoreEx.AFIDENTID objectID, int nTransferType, int nSceneID, int nLineIndex)
    {
        AFMsg.ReqAckSwapScene xData = new AFMsg.ReqAckSwapScene();
        xData.TransferType = (AFMsg.ReqAckSwapScene.Types.EGameSwapType)nTransferType;
        xData.SceneId = nSceneID;
        xData.LineId = nLineIndex;

        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqSwapScene, xData);
    }

    public void RequireProperty(AFCoreEx.AFIDENTID objectID, string strPropertyName, int nValue)
    {
        //AFMsg.ReqCommand xData = new AFMsg.ReqCommand();
        //xData.ControlId = AFToPB(objectID);
        //xData.CommandId = ReqCommand.EGameCommandType.EGCT_MODIY_PROPERTY;
        //xData.com = UnicodeEncoding.Default.GetBytes(strPropertyName);
        //xData.command_value_int = nValue;

        //MemoryStream stream = new MemoryStream();
        //Serializer.Serialize<AFMsg.ReqCommand>(stream, xData);

        //SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_CMD_PROPERTY_INT, stream);
    }

    public void RequireItem(AFCoreEx.AFIDENTID objectID, string strItemName, int nCount)
    {
        //AFMsg.ReqCommand xData = new AFMsg.ReqCommand();
        //xData.control_id = AFToPB(objectID);
        //xData.command_id = ReqCommand.EGameCommandType.EGCT_MODIY_ITEM;
        //xData.command_str_value = UnicodeEncoding.Default.GetBytes(strItemName);
        //xData.command_value_int = nCount;

        //MemoryStream stream = new MemoryStream();
        //Serializer.Serialize<AFMsg.ReqCommand>(stream, xData);

        //SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_CMD_PROPERTY_INT, stream);
    }

    public void RequireAcceptTask(AFCoreEx.AFIDENTID objectID, string strTaskID)
    {
        AFMsg.ReqAcceptTask xData = new AFMsg.ReqAcceptTask();
        xData.TaskId = strTaskID;
        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqAcceptTask, xData);
    }

    public void RequireCompeleteTask(AFCoreEx.AFIDENTID objectID, string strTaskID)
    {
        AFMsg.ReqCompeleteTask xData = new AFMsg.ReqCompeleteTask();
        xData.TaskId = strTaskID;
        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqCompeleteTask, xData);
    }

    public void RequirePickUpItem(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID nItemID)
    {
        AFMsg.ReqPickDropItem xData = new AFMsg.ReqPickDropItem();
        xData.ItemGuid = AFToPB(nItemID);

        SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqPickItem, xData);
    }
}
}