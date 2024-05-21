import React, { useState } from 'react';
import axios from 'axios';

const Home = () => {

    const [amazonItems, setAmazonItems] = useState([]);
    const [searchText, setSearchText] = useState('');

    const onSearchClick = async () => {
        const { data } = await axios.get(`/api/amazonscraping/scrape?query=${searchText}`);
        setAmazonItems(data);
    }

    return (
        <div className="container" style={{ marginTop: 80 }}>
            <div className="mb-3">
                <input type="text" onChange={e => setSearchText(e.target.value)}
                    value={searchText}
                    className="form-control" placeholder="Search..." />
                <button className="btn btn-primary" onClick={onSearchClick}>Search</button>
            </div>

            {!!amazonItems.length &&
                <table className="table table-bordered">
                    <thead>
                        <tr>
                            <th>Image</th>
                            <th>Title</th>
                            <th>Price</th>
                        </tr>
                    </thead>
                    <tbody>
                        {amazonItems.map(item => (
                            <tr key={item.title}>
                                <td><img src={item.image} style={{ height: 150 }} alt={item.title} className="img-thumbnail" /></td>
                                <td>
                                    <a href={item.url} target='_blank'>
                                        {item.title}
                                    </a>
                                </td>
                                <td>{item.price}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            }
        </div>
    );
};

export default Home;