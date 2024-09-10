import { createContext } from "react";

export const AuthContext = createContext({});

// export function AuthContextProvider({ children }) {

//     const token = "";

//     localStorage.setItem("@bitfinance:token-1.0.0", token);

//     return (
//         <AuthContext.Provider value={{}}>
//             {children}
//         </AuthContext.Provider>
//     )
// }
