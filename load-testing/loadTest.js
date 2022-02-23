import http from 'k6/http';
import {sleep} from 'k6';

export default function () {

    const options = {
        headers: {
            Authorization: `Bearer ${__ENV.ACCESS_TOKEN}`,
        },
    };

    http.get(__ENV.ENDPOINT, options);
    sleep(1);
}
