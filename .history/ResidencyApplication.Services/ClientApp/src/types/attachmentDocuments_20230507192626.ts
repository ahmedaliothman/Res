export interface IFileAttachment { 

    id?:number, 
    ApprovedLetterForResidencyRenewal?:File,
    SalaryCertification?:File,
    CivilIdCopy?:File,
    PassportCopy?:File,
    OtherRelatedDocuments?:FileList,
    ApplicationNumber?:number,
    UserId?:number,
    CreatedDate?:Date,
    UpdatedDate?:Date,
    navigationUrl?:string,
    history?:any

  }

  export interface IAttachment { 

    id?:number, 
    approvedLetterForResidencyRenewal?:string,
    salaryCertification?:string,
    civilIdCopy?:string,
    passportCopy?:string,
    otherRelatedDocuments?:string,
    applicationNumber?:number,
    userId?:number,
    createdDate?:Date,
    updatedDate?:Date,
    navigationUrl?:string

  }