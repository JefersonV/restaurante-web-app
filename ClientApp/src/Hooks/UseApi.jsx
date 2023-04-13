import { useState } from 'react';

const UseApi = () => {
  const [data, setData] = useState(null);
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);

  const fetchData = async (url, method, body) => {
    setIsLoading(true);

    try {
      const response = await fetch(url, {
        method,
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      const responseData = await response.json();
      setData(responseData);
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  const get = async (url) => {
    await fetchData(url, 'GET');
  };

  const post = async (url, body) => {
    await fetchData(url, 'POST', body);
  };

  const put = async (url, body) => {
    await fetchData(url, 'PUT', body);
  };

  const patch = async (url, body) => {
    await fetchData(url, 'PATCH', body);
  };

  const remove = async (url) => {
    await fetchData(url, 'DELETE');
  };

  return {
    data,
    error,
    isLoading,
    get,
    post,
    put,
    patch,
    remove
  };
};

export default UseApi;