import React from 'react';

class Title extends React.Component {
  static propTypes = {
    title: React.PropTypes.string.isRequired,
  }

  handleClick(saying) {
    alert(saying);

  }

    render() {
        const { title, id } = this.props;

      return (
            <div>
                <img src={`http://lorempixel.com/600/400?id=${id}`} />
                <h1 className="title" onClick={this.handleClick.bind(this, title)}>{title}</h1>
            </div>
        );
  }
}

export default Title;
