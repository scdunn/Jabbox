export interface AuthResponse {
  userName: string;
  isAuthSuccessful: boolean;
  errorMessage: string;
  token: string;
}
