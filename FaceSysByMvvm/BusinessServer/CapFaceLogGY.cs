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
public partial class CapFaceLogGY : TBase
{
  private string _ID;
  private string _ChannelID;
  private long _time;
  private long _timeIn;
  private long _timeOut;
  private int _quality;
  private int _age;
  private int _gender;
  private string _channelname;
  private string _zname;

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

  public string ChannelID
  {
    get
    {
      return _ChannelID;
    }
    set
    {
      __isset.ChannelID = true;
      this._ChannelID = value;
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

  public long TimeIn
  {
    get
    {
      return _timeIn;
    }
    set
    {
      __isset.timeIn = true;
      this._timeIn = value;
    }
  }

  public long TimeOut
  {
    get
    {
      return _timeOut;
    }
    set
    {
      __isset.timeOut = true;
      this._timeOut = value;
    }
  }

  public int Quality
  {
    get
    {
      return _quality;
    }
    set
    {
      __isset.quality = true;
      this._quality = value;
    }
  }

  public int Age
  {
    get
    {
      return _age;
    }
    set
    {
      __isset.age = true;
      this._age = value;
    }
  }

  public int Gender
  {
    get
    {
      return _gender;
    }
    set
    {
      __isset.gender = true;
      this._gender = value;
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

  public string Zname
  {
    get
    {
      return _zname;
    }
    set
    {
      __isset.zname = true;
      this._zname = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool ID;
    public bool ChannelID;
    public bool time;
    public bool timeIn;
    public bool timeOut;
    public bool quality;
    public bool age;
    public bool gender;
    public bool channelname;
    public bool zname;
  }

  public CapFaceLogGY() {
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
              ChannelID = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              Time = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              TimeIn = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              TimeOut = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              Quality = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I32) {
              Age = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I32) {
              Gender = iprot.ReadI32();
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
          case 10:
            if (field.Type == TType.String) {
              Zname = iprot.ReadString();
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
      TStruct struc = new TStruct("CapFaceLogGY");
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
      if (ChannelID != null && __isset.ChannelID) {
        field.Name = "ChannelID";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ChannelID);
        oprot.WriteFieldEnd();
      }
      if (__isset.time) {
        field.Name = "time";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Time);
        oprot.WriteFieldEnd();
      }
      if (__isset.timeIn) {
        field.Name = "timeIn";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TimeIn);
        oprot.WriteFieldEnd();
      }
      if (__isset.timeOut) {
        field.Name = "timeOut";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(TimeOut);
        oprot.WriteFieldEnd();
      }
      if (__isset.quality) {
        field.Name = "quality";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Quality);
        oprot.WriteFieldEnd();
      }
      if (__isset.age) {
        field.Name = "age";
        field.Type = TType.I32;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Age);
        oprot.WriteFieldEnd();
      }
      if (__isset.gender) {
        field.Name = "gender";
        field.Type = TType.I32;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Gender);
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
      if (Zname != null && __isset.zname) {
        field.Name = "zname";
        field.Type = TType.String;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Zname);
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
    StringBuilder __sb = new StringBuilder("CapFaceLogGY(");
    bool __first = true;
    if (ID != null && __isset.ID) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("ID: ");
      __sb.Append(ID);
    }
    if (ChannelID != null && __isset.ChannelID) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("ChannelID: ");
      __sb.Append(ChannelID);
    }
    if (__isset.time) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Time: ");
      __sb.Append(Time);
    }
    if (__isset.timeIn) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("TimeIn: ");
      __sb.Append(TimeIn);
    }
    if (__isset.timeOut) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("TimeOut: ");
      __sb.Append(TimeOut);
    }
    if (__isset.quality) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Quality: ");
      __sb.Append(Quality);
    }
    if (__isset.age) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Age: ");
      __sb.Append(Age);
    }
    if (__isset.gender) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Gender: ");
      __sb.Append(Gender);
    }
    if (Channelname != null && __isset.channelname) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Channelname: ");
      __sb.Append(Channelname);
    }
    if (Zname != null && __isset.zname) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Zname: ");
      __sb.Append(Zname);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

