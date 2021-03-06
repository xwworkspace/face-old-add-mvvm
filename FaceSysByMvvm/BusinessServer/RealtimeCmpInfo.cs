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
public partial class RealtimeCmpInfo : TBase
{
  private string _CapID;
  private string _ObjID;
  private byte[] _CapImg;
  private byte[] _ObjImg;
  private string _name;
  private string _channel;
  private long _time;
  private int _type;
  private int _score;

  public string CapID
  {
    get
    {
      return _CapID;
    }
    set
    {
      __isset.CapID = true;
      this._CapID = value;
    }
  }

  public string ObjID
  {
    get
    {
      return _ObjID;
    }
    set
    {
      __isset.ObjID = true;
      this._ObjID = value;
    }
  }

  public byte[] CapImg
  {
    get
    {
      return _CapImg;
    }
    set
    {
      __isset.CapImg = true;
      this._CapImg = value;
    }
  }

  public byte[] ObjImg
  {
    get
    {
      return _ObjImg;
    }
    set
    {
      __isset.ObjImg = true;
      this._ObjImg = value;
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


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool CapID;
    public bool ObjID;
    public bool CapImg;
    public bool ObjImg;
    public bool name;
    public bool channel;
    public bool time;
    public bool type;
    public bool score;
  }

  public RealtimeCmpInfo() {
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
              CapID = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              ObjID = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              CapImg = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              ObjImg = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              Name = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              Channel = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I64) {
              Time = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I32) {
              Type = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I32) {
              Score = iprot.ReadI32();
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
      TStruct struc = new TStruct("RealtimeCmpInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (CapID != null && __isset.CapID) {
        field.Name = "CapID";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CapID);
        oprot.WriteFieldEnd();
      }
      if (ObjID != null && __isset.ObjID) {
        field.Name = "ObjID";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ObjID);
        oprot.WriteFieldEnd();
      }
      if (CapImg != null && __isset.CapImg) {
        field.Name = "CapImg";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(CapImg);
        oprot.WriteFieldEnd();
      }
      if (ObjImg != null && __isset.ObjImg) {
        field.Name = "ObjImg";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(ObjImg);
        oprot.WriteFieldEnd();
      }
      if (Name != null && __isset.name) {
        field.Name = "name";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Name);
        oprot.WriteFieldEnd();
      }
      if (Channel != null && __isset.channel) {
        field.Name = "channel";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Channel);
        oprot.WriteFieldEnd();
      }
      if (__isset.time) {
        field.Name = "time";
        field.Type = TType.I64;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Time);
        oprot.WriteFieldEnd();
      }
      if (__isset.type) {
        field.Name = "type";
        field.Type = TType.I32;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Type);
        oprot.WriteFieldEnd();
      }
      if (__isset.score) {
        field.Name = "score";
        field.Type = TType.I32;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Score);
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
    StringBuilder __sb = new StringBuilder("RealtimeCmpInfo(");
    bool __first = true;
    if (CapID != null && __isset.CapID) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("CapID: ");
      __sb.Append(CapID);
    }
    if (ObjID != null && __isset.ObjID) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("ObjID: ");
      __sb.Append(ObjID);
    }
    if (CapImg != null && __isset.CapImg) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("CapImg: ");
      __sb.Append(CapImg);
    }
    if (ObjImg != null && __isset.ObjImg) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("ObjImg: ");
      __sb.Append(ObjImg);
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
    __sb.Append(")");
    return __sb.ToString();
  }

}

