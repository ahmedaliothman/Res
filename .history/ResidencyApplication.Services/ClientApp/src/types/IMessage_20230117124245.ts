import { ToastMessageStatus } from "../Enums/Enums";

export interface IMessage {
    message?: string;
    status?:ToastMessageStatus;
}