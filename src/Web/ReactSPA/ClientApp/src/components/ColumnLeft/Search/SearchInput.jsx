import React, { Component } from 'react';
import SearchIcon from '@material-ui/icons/Search';
import './SearchInput.css';

class SearchInput extends Component {
    render() {
        return <div className="search-input">
            <div className="search-input-wrapper">
                <SearchIcon className='search-input-icon' />
                <div id="search-inputbox" />
            </div>
        </div>
    }
}

export default SearchInput;