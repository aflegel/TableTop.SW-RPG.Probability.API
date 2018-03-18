import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';

export default class About extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
			<h1>About</h1>
        </div>;
    }
}
