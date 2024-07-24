export interface User {
    username: string;
    displayName: string;
    token: string;
    image?: string;
}

export interface UserFormValues {
    displayName?: string;
    email: string;
    password: string;
    username?: string;
}