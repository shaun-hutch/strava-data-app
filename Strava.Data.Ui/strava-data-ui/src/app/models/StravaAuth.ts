export interface StravaAuth {
    TokenType: string;
    ExpiresAt: number;
    ExpiresIn: number;
    RefreshToken: string;
    AccessToken: string;
    ExpiryUtc: Date;
    ExpiryLocal: Date;
}