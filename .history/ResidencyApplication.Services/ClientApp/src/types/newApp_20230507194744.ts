export interface INewAppState  {    
    applicationNumber?: number,
    userId?: number,
    applicationStatusId?: number,
    applicationTypeId?: number,
    applicationDate?: Date,
    isActive?: boolean,
    remark?: string,
    stepNo?: number,
    navigationUrl?:string,
    history?:any
}
