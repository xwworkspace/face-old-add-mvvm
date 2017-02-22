/**
 * Autogenerated by Thrift Compiler (0.9.3)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;


#if !SILVERLIGHT
[Serializable]
#endif
public partial class CmpFaceLogWidthImg : TBase
{
  private string _ID;
  private string _name;
  private string _channel;
  private long _time;
  private int _type;
  private int _score;
  private byte[] _img;
  private string _tcUuid;
  private string _channelname;

  public string ID
  {
    get
    {
      return _ID;
    }
    set
    {
      __isset.ID = true;
      this._ID = value;
    }
  }

  public string Name
  {
    get
    {
      return _name;
    }
    set
    {
      __isset.name = true;
      this._name = value;
    }
  }

  public string Channel
  {
    get
    {
      return _channel;
    }
    set
    {
      __isset.channel = true;
      this._channel = value;
    }
  }

  public long Time
  {
    get
    {
      return _time;
    }
    set
    {
      __isset.time = true;
      this._time = value;
    }
  }

  public int Type
  {
    get
    {
      return _type;
    }
    set
    {
      __isset.type = true;
      this._type = value;
    }
  }

  public int Score
  {
    get
    {
      return _score;
    }
    set
    {
      __isset.score = true;
      this._score = value;
    }
  }

  public byte[] Img
  {
    get
    {
      return _img;
    }
    set
    {
      __isset.img = true;
      this._img = value;
    }
  }

  public string TcUuid
  {
    get
    {
      return _tcUuid;
    }
    set
    {
      __isset.tcUuid = true;
      this._tcUuid = value;
    }
  }

  public string Channelname
  {
    get
    {
      return _channelname;
    }
    set
    {
      __isset.channelname = true;
      this._channelname = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool ID;
    public bool name;
    public bool channel;
    public bool time;
    public bool type;
    public bool score;
    public bool img;
    public bool tcUuid;
    public bool channelname;
  }

  public CmpFaceLogWidthImg() {
  }

  public void Read (TProtocol iprot)
  {
    iprot.IncrementRecursionDepth();
    try
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              ID = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Name = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Channel = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              Time = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              Type = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              Score = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.String) {
              Img = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.String) {
              TcUuid = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.String) {
              Channelname = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }
    finally
    {
      iprot.DecrementRecursionDepth();
    }
  }

  public void Write(TProtocol oprot) {
    oprot.IncrementRecursionDepth();
    try
    {
      TStruct struc = new TStruct("CmpFaceLogWidthImg");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (ID != null && __isset.ID) {
        field.Name = "ID";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ID);
        oprot.WriteFieldEnd();
      }
      if (Name != null && __isset.name) {
        field.Name = "name";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Name);
        oprot.WriteFieldEnd();
      }
      if (Channel != null && __isset.channel) {
        field.Name = "channel";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Channel);
        oprot.WriteFieldEnd();
      }
      if (__isset.time) {
        field.Name = "time";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Time);
        oprot.WriteFieldEnd();
      }
      if (__isset.type) {
        field.Name = "type";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Type);
        oprot.WriteFieldEnd();
      }
      if (__isset.score) {
        field.Name = "score";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Score);
        oprot.WriteFieldEnd();
      }
      if (Img != null && __isset.img) {
        field.Name = "img";
        field.Type = TType.String;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Img);
        oprot.WriteFieldEnd();
      }
      if (TcUuid != null && __isset.tcUuid) {
        field.Name = "tcUuid";
        field.Type = TType.String;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TcUuid);
        oprot.WriteFieldEnd();
      }
      if (Channelname != null && __isset.channelname) {
        field.Name = "channelname";
        field.Type = TType.String;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Channelname);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }
    finally
    {
      oprot.DecrementRecursionDepth();
    }
  }

  public override string ToString() {
    StringBuilder __sb = new StringBuilder("CmpFaceLogWidthImg(");
    bool __first = true;
    if (ID != null && __isset.ID) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("ID: ");
      __sb.Append(ID);
    }
    if (Name != null && __isset.name) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Name: ");
      __sb.Append(Name);
    }
    if (Channel != null && __isset.channel) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Channel: ");
      __sb.Append(Channel);
    }
    if (__isset.time) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Time: ");
      __sb.Append(Time);
    }
    if (__isset.type) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Type: ");
      __sb.Append(Type);
    }
    if (__isset.score) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Score: ");
      __sb.Append(Score);
    }
    if (Img != null && __isset.img) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Img: ");
      __sb.Append(Img);
    }
    if (TcUuid != null && __isset.tcUuid) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("TcUuid: ");
      __sb.Append(TcUuid);
    }
    if (Channelname != null && __isset.channelname) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Channelname: ");
      __sb.Append(Channelname);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

