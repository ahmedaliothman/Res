import { ToastMessageStatus } from "./Enums";

export interface IMessage {
    message?: string;
    status?:ToastMessageStatus;
}