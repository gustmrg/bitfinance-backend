import { useEffect, useState } from "react";
import axios, { AxiosError, AxiosResponse } from "axios";

interface UseAxiosProps {
    url: string;
    options?: object;
}

interface UseAxiosResult<T> {
    data: T | null;
    error: AxiosError | null;
    loading: boolean;
}

export function useAxios<T>({ url, options = {} }: UseAxiosProps): UseAxiosResult<T> {
    const [data, setData] = useState<T | null>(null);
    const [error, setError] = useState<AxiosError | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response: AxiosResponse<T> = await axios(url, options);
                setData(response.data);
            } catch (error) {
                setError(error);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [url, options]);

    return { data, error, loading };
}