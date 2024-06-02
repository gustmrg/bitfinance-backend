import { useEffect, useState } from "react"
import axios from "axios"

export function Bills() {
    const [isAuthorized, setIsAuthorized] = useState<boolean>(false)

    useEffect(() => {
        const token = localStorage.getItem('access_token');

        if (token !== null) {
            const config = {
                headers: { Authorization: `Bearer ${token}` }
            };

            axios.get(import.meta.env.API_URL + '/bills', config)
                .then(res => {
                    console.log(res.data);
                    setIsAuthorized(true);
                })
                .catch(err => console.error(err));
        }
    }, [])

    return (
        <>
            {isAuthorized ? <p>You are authorized to see this page.</p> : <p>You have no permission to this page.</p>}
        </>
    )
}